using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vezeeta.Core;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models;

namespace Vezeeta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationsController : ControllerBase
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;
        public static IWebHostEnvironment _environment;

        public AuthenticationsController(IUnitOfWork UnitOfWork, IMapper mapper, IWebHostEnvironment environment)
        {
            _mapper = mapper;
            _UnitOfWork = UnitOfWork;
            _environment = environment;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] UserRegistrationDto UserDto)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<ApplicationUser>(UserDto);
                if ( !(UserDto.Image == null || UserDto.Image.Length == 0))
                {
                    string uploadsfile = Path.Combine(_environment.WebRootPath, "Images");
                    string uniquefilaname = Guid.NewGuid().ToString() + "_" + UserDto.Image.FileName;
                    string filepath = Path.Combine(uploadsfile, uniquefilaname);

                    using (var fileStream = new FileStream(filepath, FileMode.Create))
                    {
                        UserDto.Image.CopyTo(fileStream);
                        fileStream.Close();
                    }
                    user.Image = uniquefilaname;
                }

                user.AccountType = "Patient";
                user.UserName = UserDto.FirstName + UserDto.LastName;

                var result = await _UnitOfWork.UserAuthentication.RegisterUserAsyuc(user, UserDto.Password);
                if (result.Succeeded)
                {
                    return Ok(new { Message = "Registration successful" });
                }
                return BadRequest(new { Message = "Registration failed", Errors = result.Errors });
            }
            return BadRequest(ModelState);
        }


        [HttpPost("login")]
        public async Task<IActionResult> login([FromForm] LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _UnitOfWork.UserAuthentication.CreateTokenAsync(login);
            if(result==String.Empty)
            {
                return BadRequest(new { Message = "Error in Email or Password" });
            }

            return Ok(new { Token = result});
        }

    }
}

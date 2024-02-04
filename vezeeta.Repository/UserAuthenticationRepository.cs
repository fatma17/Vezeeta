using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Vezeeta.Core;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models;
using Vezeeta.Core.Repository;

namespace vezeeta.Repository
{
    public class UserAuthenticationRepository : IUserAuthenticationRepository
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IConfiguration _configuration;
        private ApplicationUser? _user;

        public UserAuthenticationRepository (UserManager<ApplicationUser> UserManager , IConfiguration configuration  )
        {
            _UserManager= UserManager ;
            _configuration = configuration;
        }
        public async Task<IdentityResult> RegisterUserAsyuc(ApplicationUser user, String Password)
        {
            var result = await _UserManager.CreateAsync(user, Password);
            return result;
        }

        public async Task<string> CreateTokenAsync(LoginDto loginDto)
        {

            var _user = await _UserManager.FindByEmailAsync(loginDto.Email);
            if (_user != null && await _UserManager.CheckPasswordAsync(_user, loginDto.Password))
            {
                var Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, _user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, _user.Id.ToString()),
                    new Claim("id", _user.Id.ToString()),
                    new Claim(ClaimTypes.Role, _user.AccountType),
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JwtConfig:ValidIssuer"],
                    audience: _configuration["JwtConfig:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: Claims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);


            }

            return string.Empty; //Unauthorized();
        }

        public async Task Delete(ApplicationUser user)
        {
            var result = await _UserManager.DeleteAsync(user);
        }

        public async Task update(ApplicationUser user , string NewPassword)
        {
            user.PasswordHash = _UserManager.PasswordHasher.HashPassword(user, NewPassword);
            var token = await _UserManager.UpdateAsync(user);
        }


    }
}

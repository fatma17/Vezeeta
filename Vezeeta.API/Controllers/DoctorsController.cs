using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;
using vezeeta.Repository;
using Vezeeta.Core;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models;
using Vezeeta.Serivce;

namespace Vezeeta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IUnitOfWork _UnitOfWork;
        public static IWebHostEnvironment _environment;
        public DoctorsController(IUnitOfWork UnitOfWork, IWebHostEnvironment environment)
        {
            _UnitOfWork = UnitOfWork;
            _environment = environment;
        }


        [HttpGet("GetAllDoctor")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetAllDoctor([FromQuery] PaginationDto paginationDto)
        {
            if (String.IsNullOrEmpty(paginationDto.search))
            {
                return Ok(await _UnitOfWork.Doctors.GetAllSearch<DoctorWithAppointmentDto>(paginationDto.page, paginationDto.pagesize, DoctorWithAppointmentDto.DoctorAppointmentSelector));
            }
            var result = await _UnitOfWork.Doctors.GetAllSearch<DoctorWithAppointmentDto>(paginationDto.page, paginationDto.pagesize, DoctorWithAppointmentDto.DoctorAppointmentSelector, DoctorWithAppointmentDto.DoctorSearch(paginationDto.search));
            return Ok(result);
        }


        [HttpGet("GetAllDoctorsAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDoctorsAdmin([FromQuery] PaginationDto paginationDto)
        {
            if (String.IsNullOrEmpty(paginationDto.search))
            {
                return Ok(await _UnitOfWork.Doctors.GetAllSearch<DoctorDto>(paginationDto.page, paginationDto.pagesize, DoctorDto.DoctorSelector));
            }
            var result = await _UnitOfWork.Doctors.GetAllSearch(paginationDto.page, paginationDto.pagesize, DoctorDto.DoctorSelector, DoctorDto.DoctorSearch(paginationDto.search));

            return Ok(result);
        }

        [HttpGet("GetDoctorById")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var result = _UnitOfWork.Doctors.GetById<DoctorDto>(d => d.Id == id, DoctorDto.DoctorSelector);
            if (result == null)
            {
                return BadRequest(new { Message = "there is no Doctor with that id " });
            }

            string imageName = result.Image;
            string imagePath = Path.Combine("wwwroot", "Images", imageName);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound("Image not found");
            }

            string imageUrl = Url.Content($"~/images/{imageName}");

            //byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            //result.Image = Convert.ToBase64String(imageBytes);
            result.Image = imageUrl;

            return Ok(result);
        }

        [HttpPost("AddDoctor")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDoctor([FromForm] DoctorRegistrationDto DoctorDto)
        {
            if (ModelState.IsValid)
            {

                var uniquefilaname = ImagesService.Add(DoctorDto.image);

                var user = new ApplicationUser
                {
                    Image = DoctorDto.image.FileName,
                    FirstName = DoctorDto.FirstName,
                    LastName = DoctorDto.LastName,
                    Email = DoctorDto.Email,
                    PhoneNumber = DoctorDto.PhoneNumber,
                    Gender = DoctorDto.Gender,
                    DateOfBirth = DoctorDto.DateOfBirth
                };
                user.Image = uniquefilaname;
                user.AccountType = "Doctor";
                user.UserName = DoctorDto.FirstName + DoctorDto.LastName;
                Doctor doctor = new Doctor { SpecializationId = DoctorDto.SpecializationId };
                user.Doctor = doctor;

                var result = await _UnitOfWork.UserAuthentication.RegisterUserAsyuc(user, DoctorDto.Password);

                if (result.Succeeded)
                {
                    return Ok(new { Message = "Registration successful" });
                }
                return BadRequest(new { Message = "Registration failed", Errors = result.Errors });
            }
            return BadRequest(new { Message = "Registration failed", ModelState });
        }


        [HttpPut("EditDoctor")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditDoctor(int id, [FromForm] DoctorRegistrationDto DoctorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = ModelState });
            }
            var result = await _UnitOfWork.Doctors.FindAsync(b => b.Id == id, new[] { "User" });

            if (result == null)
            {
                return BadRequest(new { Message = "there is no doctor with that id " });
            }

            var ishavebooking = _UnitOfWork.Booking.Any(b => b.DoctorId == id);

            if (ishavebooking)
            {
                return BadRequest(new { Message = "The Doctor has request, can't delete " });
            }


            ImagesService.delete(result.User.Image);
            var uniquefilaname = ImagesService.Add(DoctorDto.image);

            result.User.Image = uniquefilaname;
            result.User.FirstName = DoctorDto.FirstName;
            result.User.LastName = DoctorDto.LastName;
            result.User.Email = DoctorDto.Email;
            result.User.PhoneNumber = DoctorDto.PhoneNumber;
            result.SpecializationId = DoctorDto.SpecializationId;
            result.User.Gender = DoctorDto.Gender;
            result.User.DateOfBirth = DoctorDto.DateOfBirth;
            result.User.UserName = DoctorDto.FirstName + DoctorDto.LastName;

            _UnitOfWork.UserAuthentication.update(result.User, DoctorDto.Password);
            _UnitOfWork.Save();

            return Ok(new { Message = "Update Doctor successful" });
        }


        [HttpDelete("DeletDoctor")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletDoctor(int id)
        {

            var result = await _UnitOfWork.Doctors.FindAsync(b => b.Id == id);

            if (result == null)
            {
                return BadRequest(new { Message = "There is no doctor with that id " });
            }
            var ishavebooking = _UnitOfWork.Booking.Any(Booking => Booking.DoctorId == id);

            if (ishavebooking)
            {
                return BadRequest(new { Message = "The Doctor has request, can't delete " });
            }
            var user = await _UnitOfWork.ApplicationUser.FindAsync(b => b.Id == result.UserId);

            _UnitOfWork.Doctors.Delete(result);
            _UnitOfWork.ApplicationUser.Delete(user);
            _UnitOfWork.Save();

            ImagesService.delete(user.Image);

            return Ok(new { Message = "Delete Doctor successful" });

        }


    }

}

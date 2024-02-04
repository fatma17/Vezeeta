using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vezeeta.Core;

namespace Vezeeta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StatisticsController : ControllerBase
    {
        private readonly IUnitOfWork _UnitOfWork;
     
        public StatisticsController(IUnitOfWork UnitOfWork)
        { 
            _UnitOfWork = UnitOfWork;
        }
        

        [HttpGet("GetNumOfDoctors")]
        public async Task<IActionResult> GetNumOfDoctors()
        {
            return Ok(await _UnitOfWork.Doctors.CountAsync());
        }


        [HttpGet("GetNumOfPatient")]
        public async Task<IActionResult> GetNumOfPatient()
        {
            return Ok(await _UnitOfWork.ApplicationUser.CountAsync(u => u.AccountType == "Patient"));
        }


        [HttpGet("GetNumOfRequests")]
        public IActionResult GetNumOfRequests()
        {
            return Ok( _UnitOfWork.Booking.NumOfRequests());
        }


        [HttpGet("GetTop10_Doctor")]
        public IActionResult GetTop10_Doctor()
        {
            return Ok(_UnitOfWork.Booking.GetTopDoctor(10));
        }


        [HttpGet("GetTop5_Specialization")]
        public IActionResult GetTop5_Specialization()
        {
            return Ok(_UnitOfWork.Booking.GetTopSpecilization(5));
        }

        
    }
}

using Microsoft.AspNetCore.Mvc;
using Vezeeta.Core.Models;
using Vezeeta.Core;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Vezeeta.Core.Dtos;

namespace Vezeeta.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Doctor")]
    public class AppiontmensController : ControllerBase
    {
        private readonly IUnitOfWork _UnitOfWork;
        
        public AppiontmensController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        

        [HttpPost("AddAppointment")]
        public async Task<IActionResult> AddAppointment(int price, [FromBody] List<AppointmentDto> appointmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var Id = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            
            var doctor = await _UnitOfWork.Doctors.FindAsync(d => d.UserId == Id);

            if (doctor == null)
            {
                return NotFound("Doctor isn't found");
            }

            if (doctor.Price != null)
            {
                return BadRequest("The price is already added , you can't change it");
            }

            doctor.Price = price;

            var appointments = new List<Appointment>();
            foreach (var apppintment in appointmentDto)
            {
                appointments.Add(new Appointment
                {
                    DoctorId = doctor.Id,
                    Day = apppintment.Day,
                    Times = apppintment.Times.Select(T => new Times
                    {
                        Time = T
                    }).ToList()
                });
            }
            await _UnitOfWork.Appointments.AddRangeAsync(appointments);
            _UnitOfWork.Save();

            return Ok("Appointments inserted successfully");
        }


        [HttpPut("update")]
        public async Task<IActionResult> updateAppointment(int timeId, TimeSpan NewTime)
        {
            var Id = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var doctor = await _UnitOfWork.Doctors.FindAsync(d => d.UserId == Id); 

            var result = await _UnitOfWork.Times.FindAsync(b => b.Id == timeId && b.Appointment.DoctorId == doctor.Id);

            if (result == null)
            {
                return BadRequest(new { Message = "There is no Appointment with that id " });
            }

            var ishavebooking = _UnitOfWork.Booking.Any(b => b.TimesId == timeId && b.status != Status.Cancelled);

            if (ishavebooking)
            {
                return BadRequest(new { Message = "This Appointment has booked , can't delete " });
            }

            result.Time = NewTime;

            _UnitOfWork.Times.Update(result);
            _UnitOfWork.Save();
            return Ok(new { Message = "Update time successful" });
        }


        [HttpDelete("DeleteAppointment")]
        public async Task<IActionResult> DeleteAppointment(int timeId)
        {
            var Id = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var doctor = await _UnitOfWork.Doctors.FindAsync(d => d.UserId == Id);

            var result = await _UnitOfWork.Times.FindAsync(b => b.Id == timeId && b.Appointment.DoctorId== doctor.Id);

            if (result == null)
            {
                return BadRequest(new { Message = "There is no Appointment with that id " });
            }

            var ishavebooking = _UnitOfWork.Booking.Any(b => b.TimesId == timeId);

            if (ishavebooking)
            {
                return BadRequest(new { Message = "This Appointment has booked , can't delete " });
            }

            _UnitOfWork.Times.Delete(result);
            _UnitOfWork.Save();

            return Ok(new { Message = "Delete Appointment successful" });
        }

    }
}

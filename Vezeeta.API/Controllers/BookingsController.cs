using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vezeeta.Core.Models;
using Vezeeta.Core;
using Vezeeta.Serivce;
using Microsoft.AspNetCore.Authorization;
using Vezeeta.Core.Dtos;

namespace Vezeeta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IUnitOfWork _UnitOfWork;

        public BookingsController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }


        [HttpPost("AddBooking")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> AddBooking([FromForm] BookingDto bookingDto)
        {
            var PatientId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Add Booking failed", ModelState });
            }

            bool UseCoupon = false;
            var coupon = new Coupon();
            if (bookingDto.codecoupon != null)
            {
                coupon = await _UnitOfWork.Coupons.FindAsync(c => c.DiscoundCode == bookingDto.codecoupon);
                if (coupon == null)
                {
                    return NotFound("DiscoundCode isn't found");
                }
                if (coupon.Deactivate == true)
                {
                    return BadRequest(new { Message = "DiscoundCode is Deactivate" });
                }

                var NumCompletedBookings = await _UnitOfWork.Booking.CountAsync(b =>
                                                                  b.PatientId == PatientId
                                                                  && b.status == Status.Completed);

                if (NumCompletedBookings < coupon.NumOfCompletedBookings)
                {
                    return BadRequest(new { Message = " Coupon Can't used ,The patient hasn't completed enough bookings" });
                }


                var isCouponUsed = _UnitOfWork.Booking.Any(b =>
                                                       b.PatientId == PatientId
                                                       && b.CouponId == coupon.Id);

                if (isCouponUsed)
                {
                    return BadRequest(new { Message = "The coupon is already used ,The coupon is used once " });
                }

                UseCoupon = true;
            }

            var time = _UnitOfWork.Times.FindTimeWithPrice(bookingDto.timeid);

            if (time == null)
            {
                return NotFound(new { Message = "Time isn't found" });
            }

            var isTimeBooked = _UnitOfWork.Booking.Any(b => b.TimesId == bookingDto.timeid && b.status == Status.Pending );
            if (isTimeBooked)
            {
                return BadRequest(new { Message = "The Time is Booked , please select another time " });
            }

            var booking = new Booking
            {
                status = Status.Pending,
                PriceBefore = time.price.Value,
                FinalPrice = time.price.Value,
                TimesId = bookingDto.timeid,
                PatientId = PatientId,
                DoctorId = time.doctorid,
            };

            if (UseCoupon)
            {
                var finalprice = CouponService.CalculationFinalPrice(time.price.Value, coupon.value, coupon.DiscoundType);
                booking.FinalPrice = finalprice;
                booking.DiscoundCode = bookingDto.codecoupon;
                booking.CouponId = coupon.Id;
            }

            _UnitOfWork.Booking.AddAsync(booking);
            _UnitOfWork.Save();

            return Ok(new { Message = "The Booking is successful" });
        }

        [HttpGet("GetAllBooking")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetAllBooking()
        {
            var patientId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _UnitOfWork.Booking.GetAll(b => b.PatientId == patientId, BookingOfPatientDto.BookingOfPatientSelector);
            return Ok(result);
        }

        [HttpPut("CancelBooking")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> CancelBooking(int bookingid)
        {
            var PatientId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var result = await _UnitOfWork.Booking.FindAsync(b => b.Id == bookingid && b.PatientId == PatientId);

            if (result == null)
            {
                return BadRequest(new { Message = "there is no booking with that id " });
            }
            if (result.status == Status.Completed)
            {
                return BadRequest(new { Message = "this booking is Completed , cant't cancel" });
            }
            if (result.status == Status.Cancelled)
            {
                return BadRequest(new { Message = "this booking is already cancel" });
            }
            result.status = Status.Cancelled;
            _UnitOfWork.Booking.Update(result);
            _UnitOfWork.Save();

            return Ok(new { Message = "Cancel Booking successful" });
        }


        [HttpPut("ConfirmCheckUpBooking")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> ConfirmCheckUpBooking(int BookingId)
        {
            var Id = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var doctor = await _UnitOfWork.Doctors.FindAsync(d => d.UserId == Id);
            var result = await _UnitOfWork.Booking.FindAsync(b => b.Id == BookingId && b.DoctorId == doctor.Id);

            if (result == null)
            {
                return NotFound("Bookingid isn't found");
            }
            if (result.status == Status.Cancelled)
            {
                return BadRequest("Bookingid is Cancelled");
            }
            if (result.status == Status.Completed)
            {
                return BadRequest("Bookingid is already Completed ");
            }
            result.status = Status.Completed;
            _UnitOfWork.Booking.Update(result);
            _UnitOfWork.Save();
            return Ok(new { Message = "Confirm Booking successful" });
        }


        [HttpGet("GetAllBookingDoctor")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetAllBookingDoctor([FromQuery] PaginationDto paginationDto)
        {
            var Id = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var doctor = await _UnitOfWork.Doctors.FindAsync(d => d.UserId == Id);

            if (String.IsNullOrEmpty(paginationDto.search))
            {
                return Ok(await _UnitOfWork.Booking.GetAllSearch<BookingDoctorDto>(paginationDto.page, paginationDto.pagesize, BookingDoctorDto.BookingDoctorSelector , criteria: b => b.DoctorId == doctor.Id));
            }
            var result = await _UnitOfWork.Booking.GetAllSearch<BookingDoctorDto>(paginationDto.page, paginationDto.pagesize, BookingDoctorDto.BookingDoctorSelector, BookingDoctorDto.BookingDoctorSearch(paginationDto.search) , b => b.DoctorId == doctor.Id);

            return Ok(result);
        }
    }
}

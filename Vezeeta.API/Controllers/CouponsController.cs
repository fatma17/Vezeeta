using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vezeeta.Core;
using vezeeta.Repository;
using Vezeeta.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Vezeeta.Core.Dtos;

namespace Vezeeta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CouponsController : ControllerBase
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;

        public CouponsController(IUnitOfWork UnitOfWork  , IMapper mapper)
        {
            _UnitOfWork = UnitOfWork;
            _mapper = mapper;
        }


        [HttpPost("AddCoupon")] 
        public async Task<IActionResult> AddCoupon([FromBody] CouponDto couponDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Add Coupon failed", ModelState });
            }

            if (couponDto.DiscoundType == DiscountType.Percentage && couponDto.value >= 100)
            {
                return BadRequest(new { Message = "AddCoupon failed , When DiscountType = Percentage , value can't be greater than or equal 100 " });
            }

            var CouponCode = await _UnitOfWork.Coupons.FindAsync(c => c.DiscoundCode == couponDto.DiscoundCode);

            if (CouponCode != null)
            {
                return BadRequest("DiscoundCode is used ,it must be unique , enter a new one ");
            }

            var coupon = _mapper.Map<Coupon>(couponDto);
            coupon.Deactivate = false;
            await _UnitOfWork.Coupons.AddAsync(coupon);
            var result = _UnitOfWork.Save();

            if (result > 0)
            {
                return Ok(new { Message = "Add Coupon successful" });
            }
            return BadRequest(new { Message = "Add Coupon failed , Try Again" });
        }


        [HttpPut("UpdateCoupon")]
        public async Task<IActionResult> UpdateCoupon(int id, [FromForm] CouponDto couponDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = ModelState });
            }
            var result = await _UnitOfWork.Coupons.FindAsync(b => b.Id == id);

            if (result == null)
            {
                return BadRequest(new { Message = "There is no Coupon with that id " });
            }

            var ishavebooking = _UnitOfWork.Booking.Any(b => b.CouponId == id);

            if (ishavebooking)
            {
                return BadRequest(new { Message = "This coupon is used , can't update it" });
            }

            result.DiscoundCode = couponDto.DiscoundCode;
            result.value = couponDto.value;
            result.NumOfCompletedBookings = couponDto.NumOfCompletedBookings;

            _UnitOfWork.Coupons.Update(result);
            _UnitOfWork.Save();

            return Ok(new { Message = "Update Coupon successful" });
        }


        [HttpDelete("DeleteCoupon")] 
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            var result = await _UnitOfWork.Coupons.FindAsync(b => b.Id == id);

            if (result == null)
            {
                return BadRequest(new { Message = "There is no Coupon with that id "});
            }
            var ishavebooking = _UnitOfWork.Booking.Any(b => b.CouponId == id);
            if (ishavebooking)
            {
                return BadRequest("This coupon is used , can't delete it");
            }

            _UnitOfWork.Coupons.Delete(result);
            _UnitOfWork.Save();

            return Ok(new { Message = "Delete Coupon successful" });
        }


        [HttpPut("DeactivateCoupon")] 
        public async Task<IActionResult> DeactivateCoupon(int id)
        {
            var result =  await _UnitOfWork.Coupons.FindAsync(b => b.Id == id);
            if (result == null)
            {
                return BadRequest(new { Message = "there is no Coupon with that id " });
            }
            result.Deactivate = true;
            _UnitOfWork.Coupons.Update(result);
            _UnitOfWork.Save();

            return Ok(new { Message = "Deactivate Coupon successful" });
        }
        
    }
}

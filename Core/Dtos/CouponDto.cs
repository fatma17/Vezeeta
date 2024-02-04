using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Dtos
{
    public class CouponDto
    {

        [Required, MaxLength(150)]
        public string DiscoundCode { get; set; }

        [Required]
        public DiscountType DiscoundType { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0")]
        public int value { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Value isn't possible to be negative")]
        public int NumOfCompletedBookings { get; set; }
    }
}

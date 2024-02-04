using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public Status status { get; set; }

        [Required]
        public int PriceBefore { get; set; }

        [Required]
        public int FinalPrice { get; set; }
        public string? DiscoundCode { get; set; }


        public int? CouponId { get; set; }
        public int TimesId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        
        public virtual Coupon Coupon { get; set;} 
        public virtual Times Times { get; set; }
        public virtual ApplicationUser Patient { get; set; }
        public virtual Doctor Doctor { get; set;}
        
    }
}

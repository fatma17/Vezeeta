using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Models
{
    public class ApplicationUser:IdentityUser<int>  
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        public Gender Gender { get; set; }

       // [Range(typeof(DateTime), "1/2/2004", "3/4/2004",ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string AccountType { get; set;}
        public string? Image { get; set; }

        public virtual Doctor? Doctor { get; set; }
        public virtual ICollection<Booking> Booking { get; set; }

    }
    
}

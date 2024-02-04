using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Models
{
    public class Appointment
    {
        public int Id {get; set; }
        [Required]
        public Days Day { get; set; }      
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual ICollection<Times> Times { get; set; }
    }
    
}

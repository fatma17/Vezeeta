using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public int? Price { get; set; }
        public int UserId { get; set; }
        public int SpecializationId { get; set; }

        public virtual Specialization Specialization { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Appointment> Appointment { get; set; }
        public virtual ICollection<Booking> Booking { get; set; }

    }
}

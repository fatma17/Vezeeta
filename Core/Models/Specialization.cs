using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Models
{
    public class Specialization
    {

        public int Id { get; set; }

        [Required,MaxLength(150)]
        public string Name { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}

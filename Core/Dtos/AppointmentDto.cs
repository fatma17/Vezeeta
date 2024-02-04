using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Dtos
{
    public class AppointmentDto
    {
        [Required]
        public Days Day { get; set; }

        [DataType(DataType.Time)]
        public IEnumerable<TimeSpan> Times { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Dtos
{
    public class TopDoctorDto
    {
        public string DoctorName { get; set; }
        public string DoctorImage { get; set; }
        public int NumRequest { get; set; }
        public string DoctorSpecializa { get; set; }
    }
}

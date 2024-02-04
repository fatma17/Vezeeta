using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Dtos
{
    public class DoctorRegistrationDto : RegistrationDto
    {
        [Required]
        public IFormFile image { get; set; }

        [Required]
        public int SpecializationId { get; set; }


    }
}

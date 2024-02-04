using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Dtos
{
    public class UpdateDoctorDtocs
    {

        [Required]
        public IFormFile image { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Not a valid email")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(0)(1[0125]\d{8})$", ErrorMessage = "Invalid Egyptian phone number.")]
        public string PhoneNumber { get; set; }

        [Required]
        public int SpecializationId { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format, Example date (2023-12-08) ")]
        public DateTime DateOfBirth { get; set; }








    }
}

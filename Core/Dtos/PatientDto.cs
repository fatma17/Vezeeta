using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Models;

namespace Vezeeta.Core.Dtos
{
    public class PatientDto
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public Gender gender { get; set; }
        public string dateOfBirth { get; set; }

        public static Expression<Func<ApplicationUser, PatientDto>> PatientSelector
        {
            get
            {
                return p => new PatientDto()
                {
                    Image = p.Image,
                    Name = p.UserName,
                    email = p.Email,
                    phone = p.PhoneNumber,
                    gender = p.Gender,
                    dateOfBirth = p.DateOfBirth.ToString()
                };
            }
        }
        public static Expression<Func<PatientDto, bool>> PatientSearch(string search)
        {
            return d => d.Name.Contains(search) || d.email.Contains(search) || d.phone.Contains(search);
        }
    }
}

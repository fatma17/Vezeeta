using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Models;

namespace Vezeeta.Core.Dtos
{
    public class DoctorDto
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public Gender gender { get; set; }
        public string speclization { get; set; }


        public static Expression<Func<Doctor, DoctorDto>> DoctorSelector
        {
            get
            {
                return d => new DoctorDto()
                {
                    Image = d.User.Image,
                    Name = d.User.UserName,
                    email = d.User.Email,
                    phone = d.User.PhoneNumber,
                    gender = d.User.Gender,
                    speclization = d.Specialization.Name
                };
            }
        }

        public static Expression<Func<DoctorDto, bool>> DoctorSearch(string search)
        {
            return d => d.Name.Contains(search) || d.email.Contains(search) || d.phone.Contains(search) || d.speclization.Contains(search);
        }
    }
}

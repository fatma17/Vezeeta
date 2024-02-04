using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Models;

namespace Vezeeta.Core.Dtos
{
    public class DoctorWithAppointmentDto
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public Gender gender { get; set; }
        public string speclization { get; set; }
        public int? price { get; set; }
        public IEnumerable<AppointmentDto> appointment { get; set; }

        public static Expression<Func<Doctor, DoctorWithAppointmentDto>> DoctorAppointmentSelector
        {
            get
            {
                return d => new DoctorWithAppointmentDto()
                {
                    Image = d.User.Image,
                    Name = d.User.UserName,
                    email = d.User.Email,
                    phone = d.User.PhoneNumber,
                    gender = d.User.Gender,
                    speclization = d.Specialization.Name,
                    price = d.Price,
                    appointment = d.Appointment.Select(p => new AppointmentDto
                    {
                        Day = p.Day,
                        Times = p.Times.Select(t => t.Time)
                    })
                };
            }
        }

        public static Expression<Func<DoctorWithAppointmentDto, bool>> DoctorSearch(string search)
        {
            return d => d.Name.Contains(search) || d.email.Contains(search) || d.phone.Contains(search) || d.speclization.Contains(search);
        }
    }
}

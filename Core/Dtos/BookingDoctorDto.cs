using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Models;

namespace Vezeeta.Core.Dtos
{
    public class BookingDoctorDto : PatientDto
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public Gender gender { get; set; }
        public string dateOfBirth { get; set; }
        public Days Day { get; set; }
        public string Time { get; set; }
        public static Expression<Func<Booking, BookingDoctorDto>> BookingDoctorSelector
        {
            get
            {
                return p => new BookingDoctorDto()
                {
                    Image = p.Patient.Image,
                    Name = p.Patient.UserName,
                    email = p.Patient.Email,
                    phone = p.Patient.PhoneNumber,
                    dateOfBirth = p.Patient.DateOfBirth.ToString(),
                    Time = p.Times.Time.ToString(),
                    Day = p.Times.Appointment.Day
                };
            }

        }
        public static Expression<Func<BookingDoctorDto, bool>> BookingDoctorSearch(string search)
        {
            return d => d.Time.Contains(search) || d.Day.ToString().Contains(search);
        }

    }
}

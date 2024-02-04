using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Models;

namespace Vezeeta.Core.Dtos
{
    public class PatientWithBookingDto
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public Gender gender { get; set; }
        public string dateOfBirth { get; set; }
        public IEnumerable<BookingOfPatientDto> Bookings { get; set; }

        public static Expression<Func<ApplicationUser, PatientWithBookingDto>> PatientWithBookingSelector
        {
            get
            {
                return p => new PatientWithBookingDto()
                {
                    Image = p.Image,
                    Name = p.UserName,
                    email = p.Email,
                    phone = p.PhoneNumber,
                    Bookings = p.Booking.Select(b => new BookingOfPatientDto
                    {
                        Image = b.Doctor.User.Image,
                        DoctorName = b.Doctor.User.UserName,
                        specialize = b.Doctor.Specialization.Name,
                        Day = b.Times.Appointment.Day,
                        time = b.Times.Time,
                        price = b.PriceBefore,
                        discoundcode = b.DiscoundCode,
                        finalprice = b.FinalPrice,
                        status = b.status
                    })
                };
            }
        }

        public static Expression<Func<PatientWithBookingDto, bool>> PatientSearch(string search)
        {
            return d => d.Name.Contains(search) || d.email.Contains(search) || d.phone.Contains(search);
        }
    }
}

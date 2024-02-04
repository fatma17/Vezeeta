using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Models;

namespace Vezeeta.Core.Dtos
{
    public class BookingOfPatientDto
    {
        public string Image { get; set; }
        public string DoctorName { get; set; }
        public string specialize { get; set; }
        public Days Day { get; set; }
        public TimeSpan time { get; set; }
        public int price { get; set; }
        public string? discoundcode { get; set; }
        public int finalprice { get; set; }
        public Status status { get; set; }

        public static Expression<Func<Booking, BookingOfPatientDto>> BookingOfPatientSelector
        {
            get
            {
                return b => new BookingOfPatientDto()
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
                };
            }
        }

    }
}

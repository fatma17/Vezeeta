using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Models;
using Vezeeta.Core.Repository;

namespace Vezeeta.Core
{
    public interface IUnitOfWork
    {
        IBaseRepository<ApplicationUser> ApplicationUser { get;}
        IBaseRepository<Doctor> Doctors { get; }
        IBookingRepository  Booking { get; }
        IBaseRepository<Appointment> Appointments { get; }
        ITimesRepository Times { get; }
        IBaseRepository<Coupon> Coupons { get; }
        IUserAuthenticationRepository UserAuthentication { get; }
        int Save();
    }
}

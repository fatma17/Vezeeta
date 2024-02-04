using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core;
using Vezeeta.Core.Models;
using Vezeeta.Core.Repository;
using static System.Reflection.Metadata.BlobBuilder;

namespace vezeeta.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IConfiguration _configuration;

        public IBaseRepository<ApplicationUser> ApplicationUser { get; private set; }
        public IBaseRepository<Doctor> Doctors { get; private set; }
        public IBookingRepository Booking { get; private set; }
        public IBaseRepository<Appointment> Appointments { get; private set; }
        public ITimesRepository Times { get; private set; }
        public IBaseRepository<Coupon> Coupons { get; private set; }
        public IUserAuthenticationRepository UserAuthentication { get; private set; }


        public UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _UserManager = userManager;
            _configuration = configuration;
            ApplicationUser = new BaseRepository<ApplicationUser>(_context);
            Doctors = new BaseRepository<Doctor>(_context);
            Booking = new BookingRepository(_context);
            Appointments = new BaseRepository<Appointment>(_context);
            Times = new TimesRepository(_context);
            Coupons = new BaseRepository<Coupon>(_context);

            UserAuthentication = new UserAuthenticationRepository(_UserManager,  _configuration);
        }
        public int Save()
        {
            return _context.SaveChanges();
        }

        //public void Dispose()
        //{
        //    _context.Dispose();
        //}

    }
} 

       
 
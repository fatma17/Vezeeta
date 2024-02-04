using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Vezeeta.Core;

namespace vezeeta.Repository
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser,IdentityRole<int>,int>
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        
        protected override async void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<Doctor>()
                .HasMany(b => b.Booking)
                .WithOne(b => b.Doctor)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Times>()
                .HasMany(T => T.Booking)
                .WithOne(b => b.Times)
                .OnDelete(DeleteBehavior.Restrict);

            //create user (admin)
            var appUser = new ApplicationUser
            {
                Id = 1,
                FirstName = "fatma",
                LastName = "ashraf",
                UserName = "fatmaashraf",
                Gender= Gender.Female,
                AccountType = "Admin",
                Email = "fatma@gmail.com",
                NormalizedEmail= "fatma@gmail.com".ToUpper()
            }; 

            //set user password
            PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
            appUser.PasswordHash = ph.HashPassword(appUser, "123@Abc");

            //seed user (admin)
            builder.Entity<ApplicationUser>().HasData(appUser);
           

            builder.Entity<Specialization>().HasData(
                new Specialization {Id = 1, Name = "Dermatology" },
                new Specialization {Id = 2, Name = "Dentistry" },
                new Specialization {Id = 3, Name = "Gastroenterology"});
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Times> Times { get; set; }

    }


}

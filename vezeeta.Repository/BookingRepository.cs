using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models;
using Vezeeta.Core.Repository;

namespace vezeeta.Repository
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        public BookingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<NumRequestDto> NumOfRequests()
        {
            var result = _context.Bookings
                .GroupBy(g => g.status)
                .Select(g => new NumRequestDto
                {
                    _status = g.Key, 
                    count = g.Count() 
                });
            return result;
        }
        public IEnumerable<TopDoctorDto> GetTopDoctor(int Take) 
        {
            var topDoctors = _context.Bookings
                .GroupBy(b => b.DoctorId)
                .Select(g => new
                {
                    DoctorId = g.Key,
                    CountRequest = g.Count()
                })
                .OrderByDescending(d => d.CountRequest)
                .Take(Take)
                .Join(_context.Doctors,bookings => bookings.DoctorId,doctors => doctors.Id,
                     (bookings, doctors) => new TopDoctorDto
                     {
                        DoctorImage = doctors.User.Image,
                        DoctorName = doctors.User.UserName,
                        DoctorSpecializa = doctors.Specialization.Name,
                        NumRequest = bookings.CountRequest
                     })
                .ToList();
            return topDoctors; 
        }
        public IEnumerable<TopSpecilizationDto> GetTopSpecilization(int Take)
        {
            var topDoctors = _context.Bookings
                .GroupBy(b => b.DoctorId)
                .Select(g => new
                {
                    DoctorId = g.Key,
                    CountRequest = g.Count()
                })
                .OrderByDescending(d => d.CountRequest)
                .Take(Take)
                .Join(_context.Doctors,bookings => bookings.DoctorId,doctors => doctors.Id,
                    (bookings, doctors) => new TopSpecilizationDto
                    {
                        Name = doctors.Specialization.Name,
                        count = bookings.CountRequest
                    })
                .ToList();
            return topDoctors;
        }

    }
}

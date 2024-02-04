using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models;
using Vezeeta.Core.Repository;

namespace vezeeta.Repository
{
    public class TimesRepository : BaseRepository<Times>, ITimesRepository
    {
        protected ApplicationDbContext _context;
        public TimesRepository(ApplicationDbContext context) : base(context)
        {
            _context=context;
        }

        public TimeDto FindTimeWithPrice(int id) 
        {
            var result = _context.Times
                .Where(time => time.Id == id)
                .Select(T => new TimeDto
                {
                    timeid = T.Id,
                    doctorid = T.Appointment.DoctorId,
                    price = T.Appointment.Doctor.Price
                })
                .SingleOrDefault();                                                        

            return result ;
        }
    }
}

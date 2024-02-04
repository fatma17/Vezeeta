using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models;

namespace Vezeeta.Core.Repository
{
    public interface IBookingRepository : IBaseRepository<Booking>
    {
        IEnumerable<NumRequestDto> NumOfRequests();
        IEnumerable<TopDoctorDto> GetTopDoctor(int Take);
        IEnumerable<TopSpecilizationDto> GetTopSpecilization(int Take);

    }
}

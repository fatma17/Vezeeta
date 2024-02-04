using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models;

namespace Vezeeta.Core.Repository
{
    public interface ITimesRepository: IBaseRepository<Times>
    {
        TimeDto FindTimeWithPrice(int id);
    }
}

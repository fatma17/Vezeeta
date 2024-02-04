using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models;

namespace Vezeeta.Core.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationDto,ApplicationUser>();
            CreateMap<CouponDto,Coupon>();
        }
    }
}

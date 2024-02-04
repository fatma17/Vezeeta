using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Models;

namespace Vezeeta.Core.Repository
{
    public interface IUserAuthenticationRepository
    {
        Task<IdentityResult> RegisterUserAsyuc(ApplicationUser user, String Password);
        Task<string> CreateTokenAsync(LoginDto loginDto);
        Task Delete(ApplicationUser user);
        Task update(ApplicationUser user, string NewPassword);



    }
}

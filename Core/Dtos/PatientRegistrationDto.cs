﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Dtos
{
    public class PatientRegistrationDto : UserRegistrationDto
    {
        public IFormFile? Image { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Core.Dtos
{
    public class PaginationDto
    {
        [Required]
        public int page { get; set; }

        [Required]
        public int pagesize { get; set; }
        public string? search { get; set; }
    }
}

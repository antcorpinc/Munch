using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Munch.Features.Roles
{
    public class RolesViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string RoleName { get; set; }
    }
}

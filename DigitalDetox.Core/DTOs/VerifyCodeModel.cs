using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.DTOs
{
    public class VerifyCodeModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Code { get; set; }

    }
}

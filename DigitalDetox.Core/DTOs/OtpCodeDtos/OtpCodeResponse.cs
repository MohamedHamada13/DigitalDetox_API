using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.DTOs.OtpCodeDtos
{
    public class OtpCodeResponse
    {
        public bool IsTrue { get; set; }
        public string? Message { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Entities.AuthModels
{
    public class SendOtpCodeResponse
    {
        public bool EmailIsExist { get; set; }
        public string? Message { get; set; }
    }
}

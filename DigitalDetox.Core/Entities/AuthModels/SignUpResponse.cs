using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Entities.AuthModels
{
    public class SignUpResponse
    {
        public string? Email { get; set; }
        public string? FaildMessage { get; set; }
        public bool IsSuccess { get; set; }
    }
}

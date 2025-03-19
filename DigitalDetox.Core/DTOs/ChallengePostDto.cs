using DigitalDetox.Core.Entities;
using DigitalDetox.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.DTOs
{
    public class ChallengePostDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public TimeSpan Duration { get; set; } // Read "hh:mm:ss"
    }
}

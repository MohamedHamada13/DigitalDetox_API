using DigitalDetox.Core.Entities;
using DigitalDetox.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.DTOs
{
    public class ChallengeGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime CreatedAt { get; set; }
        public ChallengeState State { get; set; }

        // Ctor for mapping retrieved challenges instead of using AutoMapper
        public ChallengeGetDto(Challenge challenge)
        {
            Id = challenge.Id;
            Title = challenge.Title;
            Description = challenge.Description;
            Duration = challenge.Duration;
            CreatedAt = challenge.CreatedAt;
            State = challenge.State;
        }
    }
}

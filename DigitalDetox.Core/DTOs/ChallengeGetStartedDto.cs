﻿using DigitalDetox.Core.Entities;
using DigitalDetox.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.DTOs
{
    public class ChallengeGetStartedDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public TimeSpan Duration { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ChallengeState State { get; set; }

        // Ctor for mapping retrieved challenges instead of using AutoMapper
        public ChallengeGetStartedDto(Challenge challenge)
        {
            Id = challenge.Id;
            Title = challenge.Title;
            Description = challenge.Description;
            Duration = challenge.Duration;
            CreatedAt = challenge.CreatedAt;
            StartDate = challenge.StartDate;
            EndDate = challenge.EndDate;
            State = challenge.State;
        }
    }
}

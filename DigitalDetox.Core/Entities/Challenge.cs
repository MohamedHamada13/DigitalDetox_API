using DigitalDetox.Core.Enums;
using DigitalDetox.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace DigitalDetox.Core.Entities
{
    public class Challenge
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartDate { get; set; } = null!; // Lazy Initialization
        public DateTime? EndDate { get; set; } = null!;  // Lazy Initialization
        public ChallengeState State { get; set; } = ChallengeState.Pending; // Configured as Pending in Configurations too

        public ICollection<UsersChallenges>? UserChallenges { get; set; } = new List<UsersChallenges>(); // NP

        // Declaring an Initial constructor very important for migrations
        public Challenge() { }

        // Ctor for mapping, Instead of AutoMapper
        public Challenge(ChallengePostDto newChallenge)
        {
            Title = newChallenge.Title;
            Description = newChallenge.Description;
            Duration = newChallenge.Duration;
        }

        // Method to start the challenge
        public void StartChallenge()
        {
            if (State == ChallengeState.Pending)
            {
                StartDate = DateTime.UtcNow;
                // "dd:hh:mm:ss'
                EndDate = StartDate.Value.Add(new TimeSpan(Duration.Hours, Duration.Minutes, Duration.Seconds, 00));
                State = ChallengeState.InProgress;
            }
        }
    
        // Method For Mapping 
        public void UpdateFromDto(ChallengePostDto ChaDto)
        {
            Title = ChaDto.Title;
            Description = ChaDto.Description;
            Duration = ChaDto.Duration;
        }
    }
}

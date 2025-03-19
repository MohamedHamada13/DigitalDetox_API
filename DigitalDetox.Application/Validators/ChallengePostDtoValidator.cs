using FluentValidation;
using DigitalDetox.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DigitalDetox.Application.Validators
{
    public class ChallengePostDtoValidator : AbstractValidator<ChallengePostDto>
    {
        public ChallengePostDtoValidator()
        {
            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("Title is required!!!")
                .MaximumLength(70).WithMessage("Title maxLength is 70");

            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("Description is required!!!")
                .MaximumLength(500).WithMessage("Description maxLength is 500");

            RuleFor(c => c.Duration)
                .GreaterThan(TimeSpan.Zero).WithMessage("Duration must be greater than 0");
        }
    }
}

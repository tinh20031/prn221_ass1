using DataAccess.DTOs.Member;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Validators.Member
{
    public class UpdateMemberValidator : AbstractValidator<UpdateMemberDto>
    {
        public UpdateMemberValidator()
        {
            RuleFor(m => m.MemberId).NotEmpty().WithMessage("Member id is required");
            RuleFor(m => m.Email).NotEmpty().WithMessage("Member email is required");
            RuleFor(m => m.Email).EmailAddress().WithMessage("Invalid email");
            RuleFor(m => m.CompanyName).NotEmpty().WithMessage("Member company name is required");
            RuleFor(m => m.City).NotEmpty().WithMessage("Member city is required");
            RuleFor(m => m.Country).NotEmpty().WithMessage("Member country is required");
            RuleFor(m => m.Password).NotEmpty().WithMessage("Member password is required");

        }
    }
}

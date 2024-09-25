using DataAccess.DTOs.Order;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Validators.Order
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderValidator()
        {
            RuleFor(o => o.MemberId).NotEmpty().WithMessage("Member id is required");
            RuleFor(o => o.Freight).NotNull().GreaterThan(0).WithMessage("Freight must > 0");
        }
    }
}

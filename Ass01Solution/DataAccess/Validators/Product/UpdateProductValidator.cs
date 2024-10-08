﻿using DataAccess.DTOs.Product;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataAccess.Validators.Product
{
    public class UpdateProductValidator : AbstractValidator<BusinessObject.Product>
    {
        public UpdateProductValidator()
        {
            RuleFor(p => p.ProductId).NotEmpty().WithMessage("Product id is required");
            RuleFor(p => p.ProductName).NotEmpty().WithMessage("Product name is required");
            RuleFor(p => p.ProductName).MaximumLength(100).WithMessage("Product name is too long");
            RuleFor(p => p.UnitPrice).GreaterThan(0).WithMessage("Unit price must > 0");

        }
    }
}

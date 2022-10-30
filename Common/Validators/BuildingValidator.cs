using Common.Entities;
using FluentValidation;

namespace Common.Validators;

public class BuildingValidator:AbstractValidator<Building>
{
    public BuildingValidator()
    {
        RuleFor(x => x.Address).NotEmpty();
    }
}
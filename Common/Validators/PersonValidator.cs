using Common.Entities;
using FluentValidation;

namespace Common.Validators;

public class PersonValidator:AbstractValidator<Person>
{
    public PersonValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}
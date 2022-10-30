using Common.Entities;
using FluentValidation;

namespace Common.Validators;

public class BookingValidator:AbstractValidator<Booking>
{
    public BookingValidator()
    {
        RuleFor(x => x.StartDate).LessThanOrEqualTo(x => x.EndDate);
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate);
        RuleFor(x => x.StartDate).NotNull();
        RuleFor(x => x.EndDate).NotNull();
        RuleFor(x => x.Person).NotNull();
        RuleFor(x => x.Room).NotNull();
    }
}
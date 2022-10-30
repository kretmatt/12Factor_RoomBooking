using Common.Entities;
using FluentValidation;

namespace Common.Validators;

public class RoomValidator:AbstractValidator<Room>
{
    public RoomValidator()
    {
        RuleFor(x => x.RoomName).NotEmpty();
    }
}
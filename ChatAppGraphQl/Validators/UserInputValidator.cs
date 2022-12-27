using ChatAppGraphQl.Queries.UserQueries;
using FluentValidation;

namespace ChatAppGraphQl.Validators {
    public class UserInputValidator : AbstractValidator<UserInput> {
        private const string USERNAME_LENGTH = "USERNAME_LENGTH";

        public UserInputValidator() {
            RuleFor(u => u.username)
                .NotEmpty()
                .MaximumLength(50)
                .WithMessage("username must be between 1 and 50 characters")
                .WithErrorCode(USERNAME_LENGTH);
        }
    }
}

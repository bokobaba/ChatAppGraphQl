using ChatAppGraphQl.Queries.CommentQueries;
using FluentValidation;

namespace ChatAppGraphQl.Validators {
    public class PostMessageInputValidator : AbstractValidator<CreateCommentInput> {
        private const string POST_MESSAGE_LENGTH = "POST_MESSAGE_LENGTH";
        public PostMessageInputValidator() {
            RuleFor(m => m.text)
                .NotEmpty()
                .MaximumLength(150)
                .WithMessage("Post character length must not be empty and less than 150")
                .WithErrorCode(POST_MESSAGE_LENGTH);

        }
    }
}

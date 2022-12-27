using AppAny.HotChocolate.FluentValidation;
using ChatAppGraphQl.Queries.UserQueries;
using ChatAppGraphQl.Middleware;
using ChatAppGraphQl.Services.UserRepository;
using ChatAppGraphQl.Validators;
using HotChocolate.AspNetCore.Authorization;

namespace ChatAppGraphQl.Data.UserData
{
    [ExtendObjectType(typeof(Mutation))]
    [Authorize]
    public class UserMutation
    {
        private readonly IUserRepository _repository;

        public UserMutation(IUserRepository repository)
        {
            _repository = repository;
        }

        [UseUser]
        public async Task<UserMutationType> CreateUser(
            [UseFluentValidation, UseValidator<UserInputValidator>] UserInput user,
            [User] string firebaseId
        ) => await _repository.Create(firebaseId, user);

        [UseUser]
        public async Task<UserMutationType> UpdateLastSeen(
            [User] string firebaseId
        ) => await _repository.UpdateLastSeen(firebaseId);

        [Authorize(Policy = "IsAdmin")]
        public async Task<bool> DeleteUser(string username)
        {
            await _repository.DeleteUser(username);
            return true;
        }
    }
}

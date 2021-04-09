using MediatR;

namespace UsersApi.Contracts
{
    public class UserSearchRequest : IRequest<UserSearchResponse>
    {
        public int UserId { get; set; }
    }
}

using MediatR;

namespace UsersApi.Contracts
{
    public class UserSearchRequest : IRequest<UserSearchResponse>
    {
        public string SearchText { get; set; }
    }
}

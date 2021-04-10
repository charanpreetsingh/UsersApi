using MediatR;

namespace UsersApi.Contracts
{
    public class UserDetailsRequest : IRequest<UserDetailsResponse>
    {
        public int UserId { get; set; }
    }
}

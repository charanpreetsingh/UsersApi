using MediatR;

namespace UsersApi.Contracts
{
    public class UserSearchByGeoRequest : IRequest<UserSearchByGeoResponse>
    {
        public string Lat { get; set; }
        public string Lon { get; set; }
    }
}

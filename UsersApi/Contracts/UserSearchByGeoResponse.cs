using System.Collections.Generic;
using UsersApi.Models;

namespace UsersApi.Contracts
{
    public class UserSearchByGeoResponse
    {
        public List<UserDetailsResponse> users { get; set; }
    }
}

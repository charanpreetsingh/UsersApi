using System.Collections.Generic;
using UsersApi.Models;

namespace UsersApi.Contracts
{
    public class UserSearchResponse
    {
        public List<UserDetailsResponse> users { get; set; }
    }
}

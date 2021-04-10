using System.Collections.Generic;
using UsersApi.Models;

namespace UsersApi.Contracts
{
    public class UserDetailsResponse
    {
        public User User { get; set; }
        public List<Todo> Todos { get; set; }
        public List<Post> Posts { get; set; }
        public List<Album> Albums { get; set; }
    }
}

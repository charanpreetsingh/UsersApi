﻿using MediatR;
using System.Collections.Generic;

namespace UsersApi.Contracts
{
    public class UserSearchRequest : IRequest<UserSearchResponse>
    {
        public List<int> SearchText { get; set; }
    }
}

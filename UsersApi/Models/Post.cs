﻿using System.Collections.Generic;

namespace UsersApi.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public List<Comment> Comments { get; set; }
    }

}

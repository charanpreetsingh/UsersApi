using System.Collections.Generic;
using UsersApi.Models;

namespace UsersApi.Contracts
{
    public class SummaryResponse
    {
        public int Users { get; set; }
        public double AvgPostPerUser { get; set; }
        public double AvgCommentPerPost { get; set; }
    }
}

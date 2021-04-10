using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using UsersApi.Contracts;
using UsersApi.Interfaces;

namespace UsersApi.Handlers
{
    public class SummaryHandler : IRequestHandler<SummaryRequest, SummaryResponse>
    {
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly ITodoService _todoService;
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly IAlbumService _albumService;
        private readonly IPhotoService _photoService;
        public SummaryHandler(ILogger logger, IUserService userService, ITodoService todoService, IPostService postService, ICommentService commentService, IAlbumService albumService, IPhotoService photoService)
        {
            _logger = logger.ForContext("SourceContext", this.GetType().Name);
            _userService = userService;
            _todoService = todoService;
            _postService = postService;
            _commentService = commentService;
            _albumService = albumService;
            _photoService = photoService;
        }
        public async Task<SummaryResponse> Handle(SummaryRequest request, CancellationToken cancellationToken)
        {
            _logger.Information("Handle Summary Report Request");
            var users = await _userService.GetUsers();
            List<int> countOfPosts = new List<int>();
            List<int> countOfComments = new List<int>();
            foreach (var user in users)
            {
                var posts = await _postService.GetPostsByUserId(user.Id);
                countOfPosts.Add(posts.Count);
                var rates = posts.GroupBy(g => g.UserId).Select(g => new { UserId = g.Key, Count = g.Count()});
                foreach (var post in posts)
                {
                    var comments = await _commentService.GetCommentsByPostId(post.Id);
                    countOfComments.Add(comments.Count);
                }                           
            }
            _logger.Information("Return Summary Report Response");
            return new SummaryResponse { Users = users.Count(), AvgPostPerUser = countOfPosts.Average(), AvgCommentPerPost = countOfComments.Average() };
        }
    }
}

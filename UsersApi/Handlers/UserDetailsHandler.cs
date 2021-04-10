using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using UsersApi.Contracts;
using UsersApi.Interfaces;

namespace UsersApi.Handlers
{
    public class UserDetailsHandler : IRequestHandler<UserDetailsRequest, UserDetailsResponse>
    {
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly ITodoService _todoService;
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly IAlbumService _albumService;
        private readonly IPhotoService _photoService;
        public UserDetailsHandler(ILogger logger, IUserService userService, ITodoService todoService, IPostService postService, ICommentService commentService, IAlbumService albumService, IPhotoService photoService)
        {
            _logger = logger.ForContext("SourceContext", this.GetType().Name);
            _userService = userService;
            _todoService = todoService;
            _postService = postService;
            _commentService = commentService;
            _albumService = albumService;
            _photoService = photoService;
        }
        public async Task<UserDetailsResponse> Handle(UserDetailsRequest request, CancellationToken cancellationToken)
        {
            _logger.Information("Handle User Details Request");
            var user = await _userService.GetUserById(request.UserId);
            var todos = await _todoService.GetTodoByUserId(request.UserId);
            var posts = await _postService.GetPostsByUserId(request.UserId);
            foreach (var post in posts)
            {
                var comments = await _commentService.GetCommentsByPostId(post.Id);
                post.Comments = comments;
            }
            var albums = await _albumService.GetAlbumsByUserId(request.UserId);
            foreach (var album in albums)
            {
                var photos = await _photoService.GetPhotosByAlbumId(album.Id);
                album.Photos = photos;
            }
            _logger.Information("Return User Details Response");
            return new UserDetailsResponse { User = user, Todos = todos, Posts = posts, Albums = albums };
        }
    }
}

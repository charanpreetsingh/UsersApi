using MediatR;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UsersApi.Contracts;
using UsersApi.Interfaces;
using UsersApi.Models;

namespace UsersApi.Handlers
{
    public class UserSearchHandler : IRequestHandler<UserSearchRequest, UserSearchResponse>
    {
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly ITodoService _todoService;
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly IAlbumService _albumService;
        private readonly IPhotoService _photoService;
        public UserSearchHandler(ILogger logger, IUserService userService, ITodoService todoService, IPostService postService, ICommentService commentService, IAlbumService albumService, IPhotoService photoService)
        {
            _logger = logger.ForContext("SourceContext", this.GetType().Name);
            _userService = userService;
            _todoService = todoService;
            _postService = postService;
            _commentService = commentService;
            _albumService = albumService;
            _photoService = photoService;
        }
        public async Task<UserSearchResponse> Handle(UserSearchRequest request, CancellationToken cancellationToken)
        {
            _logger.Information("Handle User Search Request");
            List<UserDetailsResponse> userResponse = new List<UserDetailsResponse>();
            var allUsers = await _userService.GetUsers();
            
            var users = allUsers.Where(x => request.SearchText.Contains(x.Id)).ToList();
            
            List<User> listOfUsers = new List<User>();
            Dictionary<int, User> dictUsers = new Dictionary<int, User>();

            //SearchText - 10k
            //allUsers - 50 mil
            foreach(int id in request.SearchText) //- 10000 times
            {
                dictUsers.TryGetValue(id, out User user);

                User u = allUsers.Where(x => x.Id == id).FirstOrDefault(); //50 mil - 50 mil x 10000

                if (u != null)
                    listOfUsers.Add(u);
            }

            foreach (var user in users)
            {
                var todos = await _todoService.GetTodoByUserId(user.Id);
                var posts = await _postService.GetPostsByUserId(user.Id);
                foreach (var post in posts)
                {
                    var comments = await _commentService.GetCommentsByPostId(post.Id);
                    post.Comments = comments;
                }
                var albums = await _albumService.GetAlbumsByUserId(user.Id);
                foreach (var album in albums)
                {
                    var photos = await _photoService.GetPhotosByAlbumId(album.Id);
                    album.Photos = photos;
                }
                userResponse.Add(new UserDetailsResponse { User = user, Todos = todos, Posts = posts, Albums = albums });
            }
            _logger.Information("Return User Search Response");
            return new UserSearchResponse { users = userResponse };
        }
    }
}

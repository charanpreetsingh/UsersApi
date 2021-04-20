using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using UsersApi.Contracts;
using UsersApi.Interfaces;
using System.Collections.Generic;
using UsersApi.Models;
using System.Linq;

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
        private UserDetailsResponse _userDetailsResponse;

        public UserDetailsHandler(ILogger logger, IUserService userService, ITodoService todoService, IPostService postService, ICommentService commentService, IAlbumService albumService, IPhotoService photoService)
        {
            _logger = logger.ForContext("SourceContext", this.GetType().Name);
            _userService = userService;
            _todoService = todoService;
            _postService = postService;
            _commentService = commentService;
            _albumService = albumService;
            _photoService = photoService;

            _userDetailsResponse = new UserDetailsResponse();
        }
        public async Task<UserDetailsResponse> Handle(UserDetailsRequest request, CancellationToken cancellationToken)
        {
            _logger.Information("Handle User Details Request");

            List<Task> allTasks = new List<Task>
            {
                GetUser(request.UserId),
                GetTodos(request.UserId),
                GetComments(request.UserId),
                GetPhotos(request.UserId)
            };

            await Task.WhenAll(allTasks);

            _logger.Information("Return User Details Response");

            return _userDetailsResponse;
        }

        private async Task GetUser(int userId)
        {
            _userDetailsResponse.User = await _userService.GetUserById(userId);
        }

        private async Task GetTodos(int userId)
        {
            _userDetailsResponse.Todos = await _todoService.GetTodoByUserId(userId);
        }

        private async Task GetComments(int userId)
        {
            _logger.Information("GetComments Called..");

            var posts = await _postService.GetPostsByUserId(userId);
            List<Task<Post>> listOfTasks = new List<Task<Post>>();

            foreach (var post in posts)
            {
                listOfTasks.Add(RunCommentsTasks(post));
            }

            _userDetailsResponse.Posts = (await Task.WhenAll<Post>(listOfTasks)).ToList();
        }

        private async Task GetPhotos(int userId)
        {
            _logger.Information("GetPhotos Called..");
            var albums = await _albumService.GetAlbumsByUserId(userId);
            List<Task<Album>> listOfTasks = new List<Task<Album>>();

            foreach (var album in albums)
            {
                listOfTasks.Add(RunPhotosTasks(album));
            }

            _userDetailsResponse.Albums = (await Task.WhenAll<Album>(listOfTasks)).ToList();
        }
        private async Task<Album> RunPhotosTasks(Album album)
        {
            var photos = await _photoService.GetPhotosByAlbumId(album.Id);
            album.Photos = photos;
            return album;
        }
        private async Task<Post> RunCommentsTasks(Post post)
        {
            var comments = await _commentService.GetCommentsByPostId(post.Id);
            post.Comments = comments;
            return post;
        }
    }
}
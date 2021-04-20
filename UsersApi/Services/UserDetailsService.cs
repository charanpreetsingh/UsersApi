using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersApi.Contracts;
using UsersApi.Interfaces;
using UsersApi.Models;

namespace UsersApi.Services
{
    public class UserDetailsService : IUserDetailsService
    {
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly ITodoService _todoService;
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly IAlbumService _albumService;
        private readonly IPhotoService _photoService;
        private UserDetailsResponse _userDetailsResponse;

        public UserDetailsService(ILogger logger, IUserService userService, ITodoService todoService, IPostService postService, ICommentService commentService, IAlbumService albumService, IPhotoService photoService)
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

        public async Task<UserDetailsResponse> GetUserDetailsById(int userId, bool fetchUser)
        {
            List<Task> allTasks = new List<Task>
            {
                GetTodos(userId),
                GetComments(userId),
                GetPhotos(userId)
            };

            if (fetchUser)
                allTasks.Add(GetUser(userId));

            await Task.WhenAll(allTasks);

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

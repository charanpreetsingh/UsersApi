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
            //var watch = new System.Diagnostics.Stopwatch();
            //watch.Start();
            var user = await _userService.GetUserById(request.UserId);
            var todos = await _todoService.GetTodoByUserId(request.UserId);
            var posts = await _postService.GetPostsByUserId(request.UserId);
            posts = await GetComments(posts);
            var albums = await _albumService.GetAlbumsByUserId(request.UserId);
            albums = await GetPhotos(albums);
            //watch.Stop();
            //_logger.Information($"Execution Time: {watch.ElapsedMilliseconds} ms");
            _logger.Information("Return User Details Response");
            return new UserDetailsResponse { User = user, Todos = todos, Posts = posts, Albums = albums };
        }
        public async Task<List<Album>> GetPhotos(List<Album> albums)
        {
            List<Task<Album>> listOfTasks = new List<Task<Album>>();

            foreach (var album in albums)
            {
                listOfTasks.Add(RunPhotosTasks(album));
            }
            var updatedAlbumns = await Task.WhenAll<Album>(listOfTasks);
            return updatedAlbumns.ToList();
        }
        private async Task<Album> RunPhotosTasks(Album album)
        {
            var photos = await _photoService.GetPhotosByAlbumId(album.Id);
            album.Photos = photos;
            return album;
        }
        public async Task<List<Post>> GetComments(List<Post> posts)
        {
            List<Task<Post>> listOfTasks = new List<Task<Post>>();

            foreach (var post in posts)
            {
                listOfTasks.Add(RunCommentsTasks(post));
            }
            var updatedPost = await Task.WhenAll<Post>(listOfTasks);
            return updatedPost.ToList();
        }

        private async Task<Post> RunCommentsTasks(Post post)
        {
            var comments = await _commentService.GetCommentsByPostId(post.Id);
            post.Comments = comments;
            return post;
        }
    }
}

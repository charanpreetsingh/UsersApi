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

        List<Post> myPosts;
        List<Album> myAlbums;

        public UserDetailsHandler(ILogger logger, IUserService userService, ITodoService todoService, IPostService postService, ICommentService commentService, IAlbumService albumService, IPhotoService photoService)
        {
            _logger = logger.ForContext("SourceContext", this.GetType().Name);
            _userService = userService;
            _todoService = todoService;
            _postService = postService;
            _commentService = commentService;
            _albumService = albumService;
            _photoService = photoService;

            myPosts = new List<Post>();
            myAlbums = new List<Album>();
        }
        public async Task<UserDetailsResponse> Handle(UserDetailsRequest request, CancellationToken cancellationToken)
        {
            _logger.Information("Handle User Details Request");
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            List<Task> allTasks = new List<Task>();

            var user = await _userService.GetUserById(request.UserId);
            var todos = await _todoService.GetTodoByUserId(request.UserId);
            var posts = await _postService.GetPostsByUserId(request.UserId);
            var albums = await _albumService.GetAlbumsByUserId(request.UserId);

            allTasks.Add(GetComments(posts));
            //posts = await GetComments(posts);
            allTasks.Add(GetPhotos(albums));

            await Task.WhenAll(allTasks);

            //albums = await GetPhotos(albums);
            watch.Stop();
            _logger.Information($"Execution Time: {watch.ElapsedMilliseconds} ms");
            _logger.Information("Return User Details Response");
            return new UserDetailsResponse { User = user, Todos = todos, Posts = myPosts, Albums = myAlbums };
        }

        private async Task GetComments(List<Post> posts)
        {
            _logger.Information("GetComments Called..");

            List<Task<Post>> listOfTasks = new List<Task<Post>>();

            foreach (var post in posts)
            {
                listOfTasks.Add(RunCommentsTasks(post));
            }
            var updatedPost = await Task.WhenAll<Post>(listOfTasks);
            myPosts = updatedPost.ToList();
            //return updatedPost.ToList();
        }

        private async Task GetPhotos(List<Album> albums)
        {
            _logger.Information("GetPhotos Called..");
            List<Task<Album>> listOfTasks = new List<Task<Album>>();

            foreach (var album in albums)
            {
                listOfTasks.Add(RunPhotosTasks(album));
            }
            var updatedAlbumns = await Task.WhenAll<Album>(listOfTasks);
            myAlbums = updatedAlbumns.ToList();
            //return updatedAlbumns.ToList();
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

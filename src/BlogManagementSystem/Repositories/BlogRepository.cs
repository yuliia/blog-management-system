using System;
using System.Linq;
using System.Threading.Tasks;
using BlogManagementSystem.Common;
using BlogManagementSystem.Db;
using BlogManagementSystem.Exceptions;
using BlogManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogManagementSystem.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDbContext _db;

        public BlogRepository(BlogDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        
        public async Task<Post> AddPost(Post post)
        {
            var newPost = _db.Posts.Add(post);
            await _db.SaveChangesAsync();
            return newPost.Entity;
        }

        public async Task<Comment> AddComment(Comment comment)
        {
            var post = await _db.Posts.FindAsync(comment.PostId);
            if (post == null)
            {
                throw new PostNotFoundException(comment.PostId);
            }

            var newComment = _db.Comments.Add(comment);
            await _db.SaveChangesAsync();
            return newComment.Entity;
        }

        public async Task DeletePost(Guid postId)
        {
            var post = await _db.Posts.FindAsync(postId);
            if (post == null)
            {
                throw new PostNotFoundException(postId);
            }

            var comment = await _db.Comments
                .Where(x => x.PostId == postId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
            
            if (comment != Guid.Empty)
            {
                throw new DeletingPostWithCommentsException(postId);
            }

            _db.Remove(post);
            await _db.SaveChangesAsync();
        }

        public async Task<Post> GetPost(Guid postId)
        {
            return await _db.Posts.Include(x => x.Comments)
                .SingleOrDefaultAsync(x => x.Id == postId);
        }

        public async Task<ListWithTotalCount<Post>> GetPosts(int limit, int offset)
        {
            var posts = await _db.Posts.OrderByDescending(x => x.Date)
                .Skip(offset)
                .Take(limit)
                .ToArrayAsync();

            var totalCount = await _db.Posts.CountAsync();

            return new ListWithTotalCount<Post>(posts, totalCount);
        }
    }
}
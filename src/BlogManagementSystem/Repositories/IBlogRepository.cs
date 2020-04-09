using System;
using System.Threading.Tasks;
using BlogManagementSystem.Common;
using BlogManagementSystem.Models;

namespace BlogManagementSystem.Repositories
{
    public interface IBlogRepository
    {
        Task<Post> AddPost(Post post);

        Task<Comment> AddComment(Comment comment);

        Task DeletePost(Guid postId);

        Task<Post> GetPost(Guid postId);

        Task<ListWithTotalCount<Post>> GetPosts(int limit, int offset);
    }
}
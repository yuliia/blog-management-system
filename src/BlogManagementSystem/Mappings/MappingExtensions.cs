using System.Linq;
using BlogManagementSystem.Common;
using BlogManagementSystem.Models;
using BlogManagementSystem.ViewModels;

namespace BlogManagementSystem.Mappings
{
    // In case of more mappings needed it can be replaced with AutoMapper library
    public static class MappingExtensions
    {
        public static ListWithTotalCount<PostResponse> ToResponseList(this ListWithTotalCount<Post> posts)
        {
            if (posts == null)
            {
                return null;
            }

            return new ListWithTotalCount<PostResponse>(posts.Items.Select(ToResponse).ToArray(), posts.TotalCount);
        }
        
        public static PostResponse ToResponse(this Post post)
        {
            if (post == null)
            {
                return null;
            }
            
            return new PostResponse
            {
                Comments = post.Comments?.Select(x => x.ToResponse()).ToList(),
                Date = post.Date,
                Text = post.Text,
                Title = post.Title,
                Id = post.Id
            };
        }
        
        public static CommentResponse ToResponse(this Comment comment)
        {
            if (comment == null)
            {
                return null;
            }

            return new CommentResponse
            {
                Id = comment.Id,
                Date = comment.Date,
                PostId = comment.PostId,
                Text = comment.Text
            };
        }
    }
}
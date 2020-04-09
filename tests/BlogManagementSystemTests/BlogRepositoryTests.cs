using System;
using System.Linq;
using System.Threading.Tasks;
using BlogManagementSystem.Db;
using BlogManagementSystem.Exceptions;
using BlogManagementSystem.Models;
using BlogManagementSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BlogManagementSystemTests
{
    public class BlogRepositoryTests
    {
        private async Task<BlogDbContext> DbContext()
        {
            var db = new BlogDbContext(
                new DbContextOptionsBuilder().UseInMemoryDatabase("Blog").Options);

            var comments = await db.Comments.ToArrayAsync();
            db.Comments.RemoveRange(comments);
            var posts = await db.Posts.ToArrayAsync();
            db.Posts.RemoveRange(posts);
            await db.SaveChangesAsync();
            return db;
        }
        
        [Fact]
        public async Task Can_Add_And_Get_New_Post()
        {
            var repo = new BlogRepository(await DbContext());

            var post = new Post("title", "text", DateTime.UtcNow);
            var saved = await repo.AddPost(post);
            
            Assert.NotEqual(default, saved.Id);
            Assert.Equal(post.Title, saved.Title);
            Assert.Equal(post.Text, saved.Text);
            Assert.Equal(post.Date, saved.Date);
            
            saved = await repo.GetPost(saved.Id);
                
            Assert.NotEqual(default, saved.Id);
            Assert.Equal(post.Title, saved.Title);
            Assert.Equal(post.Text, saved.Text);
            Assert.Equal(post.Date, saved.Date);
        }

        [Fact]
        public async Task Can_Add_Comment()
        {
            var repo = new BlogRepository(await DbContext());
            var post = await repo.AddPost(new Post("title", "text", DateTime.UtcNow));

            var comment = new Comment(post.Id, "comment text", DateTime.UtcNow);
            var saved = await repo.AddComment(comment);
            Assert.NotEqual(default, saved.Id);
            Assert.Equal(comment.Text, saved.Text);
            Assert.Equal(post.Id, saved.PostId);
            
            post = await repo.GetPost(post.Id);
            
            Assert.Single(post.Comments);
            var first = post.Comments.First();
            Assert.Equal(saved.Id, first.Id);
            Assert.Equal(comment.Text, first.Text);
            Assert.Equal(post.Id, first.PostId);
        }

        [Fact]
        public async Task Can_Not_Delete_Post_With_Comments()
        {
            var repo = new BlogRepository(await DbContext());
            var post = await repo.AddPost(new Post("title", "text", DateTime.UtcNow));
            await repo.AddComment(new Comment(post.Id, "comment text", DateTime.UtcNow));

            await Assert.ThrowsAsync<DeletingPostWithCommentsException>(async () => await repo.DeletePost(post.Id));
        }
        
        [Theory]
        [InlineData(0, 0, new string[0])]
        [InlineData(1, 0, new[] {"1"})]
        [InlineData(1, 1, new[] {"2"})]
        [InlineData(2, 1, new[] {"2", "3"})]
        [InlineData(5, 0, new[] {"1", "2", "3"})]
        [InlineData(5, 2, new[] {"3"})]
        public async Task Can_Get_Posts_With_Pagination(int limit, int offset, string[] expectedList)
        {
            var repo = new BlogRepository(await DbContext());
            await repo.AddPost(new Post("3", "3", DateTime.UtcNow));
            await repo.AddPost(new Post("2", "2", DateTime.UtcNow));
            await repo.AddPost(new Post("1", "1", DateTime.UtcNow));

            var posts = await repo.GetPosts(limit, offset);
            
            Assert.Equal(3, posts.TotalCount);
            Assert.Equal(expectedList.Length, posts.Items.Length);
            for (int i = 0; i < expectedList.Length; i++)
            {
                Assert.Equal(expectedList[i], posts.Items[i].Text);
            }
        }
        
        [Fact]
        public async Task Can_Get_Posts_Ordered_By_Date()
        {
            var repo = new BlogRepository(await DbContext());
            foreach (var str in new[] {"1", "2", "3"})
            {
                await repo.AddPost(new Post(str, str, DateTime.UtcNow));
            }
            var posts = await repo.GetPosts(10, 0);
            Assert.Equal(3, posts.Items.Length);
            Assert.True(posts.Items[0].Date > posts.Items[1].Date);
            Assert.True(posts.Items[1].Date > posts.Items[2].Date);
        }
    }
}
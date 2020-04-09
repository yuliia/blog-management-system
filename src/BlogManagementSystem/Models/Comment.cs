using System;
using System.ComponentModel.DataAnnotations;

namespace BlogManagementSystem.Models
{
    public class Comment
    {
        public Comment(Guid postId, string text, DateTime date)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (date > DateTime.UtcNow)
            {
                throw new ArgumentException("Date can't be in future");
            }

            PostId = postId;
            Text = text;
            Date = date;
        }
        public Guid Id { get; }

        public Guid PostId { get; }

        [Required]
        public string Text { get; set; }

        public DateTime Date { get; set; }
        
        public Post Post { get; }
    }
}
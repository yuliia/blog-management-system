using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogManagementSystem.Models
{
    public class Post
    {
        public Post(string title, string text, DateTime date)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException(nameof(title));
            }
            if (date > DateTime.UtcNow)
            {
                throw new ArgumentException("Date can't be in future");
            }
            
            Title = title;
            Text = text;
            Date = date;
        }

        public Guid Id { get; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        
        [Required]
        public string Text { get; set; }

        public DateTime Date { get; set; }
        
        public List<Comment> Comments { get; }
    }
}
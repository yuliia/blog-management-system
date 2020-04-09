using System;

namespace BlogManagementSystem.ViewModels
{
    public class CommentResponse
    {
        public Guid Id { get; set; }

        public Guid PostId { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }
    }
}
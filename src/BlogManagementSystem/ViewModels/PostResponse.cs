using System;
using System.Collections.Generic;

namespace BlogManagementSystem.ViewModels
{
    public class PostResponse
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        
        public string Text { get; set; }

        public DateTime Date { get; set; }
        
        public List<CommentResponse> Comments { get; set; }
    }
}
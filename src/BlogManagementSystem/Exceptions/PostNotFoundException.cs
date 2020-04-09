using System;
using System.Runtime.Serialization;

namespace BlogManagementSystem.Exceptions
{
    public class PostNotFoundException : Exception
    {
        protected PostNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public PostNotFoundException(Guid postId)
        {
            PostId = postId;
        }
        public PostNotFoundException(string message, Guid postId) : base(message)
        {
            PostId = postId;
        }

        public PostNotFoundException(string message, Exception innerException, Guid postId) : base(message, innerException)
        {
            PostId = postId;
        }

        public Guid PostId { get; set; }
    }
}
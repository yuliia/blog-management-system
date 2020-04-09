using System;
using System.Runtime.Serialization;

namespace BlogManagementSystem.Exceptions
{
    public class DeletingPostWithCommentsException : Exception
    {
        protected DeletingPostWithCommentsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DeletingPostWithCommentsException(Guid postId)
        {
            PostId = postId;
        }
        
        public DeletingPostWithCommentsException(string message, Guid postId) : base(message)
        {
            PostId = postId;
        }

        public DeletingPostWithCommentsException(string message, Exception innerException, Guid postId) : base(message, innerException)
        {
            PostId = postId;
        }

        public Guid PostId { get; set; }
    }
}
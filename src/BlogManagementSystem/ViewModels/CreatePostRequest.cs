using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogManagementSystem.ViewModels
{
    public class CreatePostRequest : IValidatableObject
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        
        [Required]
        public string Text { get; set; }
        
        public DateTime Date { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Date > DateTime.UtcNow)
            {
                yield return new ValidationResult("Date can't be in future");
            }
        }
    }
}
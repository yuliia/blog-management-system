using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogManagementSystem.ViewModels
{
    public class CreateCommentRequest : IValidatableObject
    {
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
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Models
{
    public class Book
    {
        //[Key]
        public int Id { get; set; }
        //[Required(ErrorMessage = "Title is required")]
        //[StringLength(50, ErrorMessage = "Title cannot be longer than 10 characters")]
        public string? Title { get; set; }
        //[Required(ErrorMessage = "Description is required")]
        //[MaxLength(150, ErrorMessage = "Description cannot be longer than 150 characters")]
        public string? Description { get; set; }
    }

    public class BookValidator : AbstractValidator<Book>
    { 
        public BookValidator() {
            RuleFor(x => x.Title).NotEmpty().Length(1,50).WithMessage("Enter correct title");
            RuleFor(x => x.Description).NotEmpty().Length(10,150).WithMessage("Enter correct description");
        }
    }

    public static class Extensions
    {
        public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
        {
            foreach (var error in result.ErrorMessage)
            {
                modelState.AddModelError(result.ErrorMessage,result.ErrorMessage);
            }
        }
    }
}

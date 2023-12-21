using BookStoreAPI.Filters;
using BookStoreAPI.Models;
using BookStoreAPI.Repository;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace BookStoreAPI.Controllers
{
    [ApiController]
    [MySampleAsyncActionFilter("BooksController")]

    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BooksController> _logger;
        private readonly IValidator<Book> _validator;

        public BooksController(IBookRepository bookRepository, ILogger<BooksController> logger, IValidator<Book> validator) {
            _bookRepository = bookRepository;   
            _logger = logger;
            _validator = validator;
        }

        [HttpGet]
        [Route("GetBooks")]
        //[MySampleResourceFilter("Books")]
        //[MySampleActionFilter("Books", -10)]
        //[ServiceFilter(typeof(MySampleResultFilterAttribute))]
        //[TypeFilter(typeof(MySampleResultFilterAttribute), Arguments = new object[] {"Action"})]
        public IActionResult GetBooks(string searchValue, int pageNo, int pageSize, string sortColumn, string sortOrder)
        {
            try
            {
                return Ok(_bookRepository.GetAllBooks(searchValue, pageNo, pageSize, sortColumn, sortOrder));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Log from BooksController {ex.Message}");
                return BadRequest("Failed to get books details");
            }
        }

        [HttpGet]
        [Route("GetBookById")]
        //[MySampleAsyncActionFilter("BooksById")]
        public IActionResult GetBookById(int ID)
        {
            try
            {
                return Ok(_bookRepository.GetBookById(ID));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Log from BooksController {ex.Message}");
                return BadRequest("Failed to get book by id");
            }
        }

        [HttpPost]
        [Route("AddBook")]
        public IActionResult AddNewBook(Book book)
        {
            var result = _validator.Validate(book);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return ValidationProblem();
            }

            try
            {
                var response = _bookRepository.AddBook(book);
                if(response.Equals(true))
                {
                    return Ok("Book added successfully");
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Log from BooksController {ex.Message}");
                return BadRequest("Failed to add book");
            }
        }

        [HttpPut]
        [Route("EditBook")]
        public IActionResult EditBook(int id, Book book)
        {
            var result = _validator.Validate(book);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return ValidationProblem();
            }

            try
            {
                var response = _bookRepository.EditBook(id, book);
                if (response.Equals(true))
                {
                    return Ok("Book updated successfully");
                }
                else
                {
                    return BadRequest("Failed to update book");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Log from BooksController {ex.Message}");
                return BadRequest("Failed to update book");
            }
        }

        [HttpDelete]
        [Route("DeleteBook")]
        public IActionResult DeleteBook(int id)
        {
            try
            {
                var response = _bookRepository.DeleteBook(id);
                if (response.Equals(true))
                {
                    return Ok("Book deleted successfully");
                }
                else
                {
                    return BadRequest("Failed to delete book");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Log from BooksController {ex.Message}");
                return BadRequest("Failed to delete book");
            }
        }
    }
}

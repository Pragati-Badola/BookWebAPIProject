using BookStoreAPI.Models;
using BookStoreAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.Controllers
{
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BooksController> _logger;
        public BooksController(IBookRepository bookRepository, ILogger<BooksController> logger) {
            _bookRepository = bookRepository;   
            _logger = logger;
        }

        [HttpGet]
        [Route("GetBooks")]
        public IActionResult GetBooks()
        {
            try
            {
                return Ok(_bookRepository.GetAllBooks());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Log from BooksController {ex.Message}");
                return BadRequest("Failed to get books details");
            }
        }

        [HttpGet]
        [Route("GetBookById")]
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
    }
}

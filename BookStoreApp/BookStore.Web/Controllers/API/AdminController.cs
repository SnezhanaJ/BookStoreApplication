using BookStore.Domain.Domain;
using BookStore.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IBooksService _booksService;
        private readonly IAuthorService _authorService;
        private readonly IPublisherService _publisherService;
        private readonly IOrderService _orderService;

        public AdminController(IBooksService booksService, IAuthorService authorService, IPublisherService publisherService, IOrderService orderService)
        {
            _booksService = booksService;
            _authorService = authorService;
            _publisherService = publisherService;
            _orderService = orderService;
        }

        [HttpGet("[action]")]
        public List<Order> GetAllActiveOrders()
        {
            return this._orderService.GetAllOrders();
        }
        [HttpPost("[action]")]
        public Order GetDetailsForOrder(BaseEntity model)
        {
            return _orderService.GetOrderDetails(model);
        }

        [HttpGet("[action]")]
        public List<Books> GetAllBooks()
        {
            return this._booksService.GetAll();
        }

        [HttpGet("[action]")]
        public List<Author> GetAllAuthors()
        {
            return this._authorService.GetAll();
        }

        [HttpGet("[action]")]
        public List<Publisher> GetAllPublishers()
        {
            return this._publisherService.GetAll();
        }


        [HttpPost("[action]")]
        public Author GetAuthorDetails(BaseEntity id)
        {
            return this._authorService.GetDetailsWithBaseEntity(id);
        }

        [HttpPost("[action]")]
        public Publisher GetPublishersDetails(BaseEntity id)
        {
            return this._publisherService.GetDetailsWithBaseEntity(id);
        }
        [HttpPost("[action]")]
        public Books GetBooksDetails(BaseEntity id)
        {
            return this._booksService.GetDetailsWithBaseEntity(id);
        }

        [HttpPost("[action]")]

        public IActionResult EditPublisher(Publisher publisher)
        {

            if (ModelState.IsValid)
            {
                var result = _publisherService.UpdatePublisher(publisher);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Failed to update author");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("[action]")]

        public IActionResult EditAuthor(Author author)
        {

            if (ModelState.IsValid)
            {
                var result =_authorService.UpdateAuthor(author);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Failed to update author");
                }
            }
            return BadRequest(ModelState);
        }


        [HttpPost("[action]")]

        public IActionResult EditBooks(Books author)
        {

            if (ModelState.IsValid)
            {
                var result = _booksService.UpdateBook(author);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Failed to update author");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("DeleteAuthor")]
        public IActionResult DeleteAuthor(BaseEntity id)
        {
            if (ModelState.IsValid)
            {
                var result = _authorService.DeleteAuthorBoolean(id);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Faild to delete author");
                }
            }
            return BadRequest(ModelState) ;
        }

        [HttpPost("[action]")]
        public IActionResult CreateAuthor(Author author)
        {
            _authorService.CreateNewAuthor(author);
            return Ok();
        }

        [HttpPost("DeletePublisher")]
        public IActionResult DeletePublisher(BaseEntity id)
        {
            if (ModelState.IsValid)
            {
                var result = _publisherService.DeletePublisherBoolean(id);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Faild to delete author");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("[action]")]
        public IActionResult CreatePublisher(Publisher author)
        {
            _publisherService.CreateNewPublisher(author);
            return Ok();
        }

        [HttpPost("DeleteBooks")]
        public IActionResult DeleteBooks(BaseEntity id)
        {
            if (ModelState.IsValid)
            {
                var result = _booksService.DeleteBooksBoolean(id);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Faild to delete author");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("[action]")]
        public IActionResult CreateBooks(Books author)
        {
            _booksService.CreateNewBook(author);
            return Ok();
        }

        [HttpPost("[action]")]
        public bool ImportAllUsers(List<Books> model)
        {
            bool status = true;

            foreach (var item in model)
            {
                var existingBook = _booksService.GetAll().FirstOrDefault(b => b.Title == item.Title);
               // var existingPublisher = _publisherService.GetAll().FirstOrDefault(b => b.Name == item.Publisher.Name);
                //var existingAuthor = _authorService.GetAll().FirstOrDefault(b => b.FirstName == item.Author.FirstName && b.LastName == item.Author.LastName);



                if (existingBook == null)
                {

                    var book = new Books
                    {
                        Title = item.Title,
                        BookImage = item.BookImage,
                        Price = item.Price,
                        ReleaseDate = item.ReleaseDate,
                       
                    };
                    var existingPublisher = _publisherService.GetAll().FirstOrDefault(p => p.Name == item.Publisher.Name);
                    var existingAuthor = _authorService.GetAll().FirstOrDefault(a => a.FirstName == item.Author.FirstName && a.LastName == item.Author.LastName);

                    // Check if Publisher and Author exist
                    if (existingPublisher == null)
                    {
                        // Publisher does not exist, so add it
                        book.Publisher = item.Publisher;
                    }
                    else
                    {
                        // Publisher exists, assign the existing one
                        book.Publisher = existingPublisher;
                    }

                    if (existingAuthor == null)
                    {
                        // Author does not exist, so add it
                        book.Author = item.Author;
                    }
                    else
                    {
                        // Author exists, assign the existing one
                        book.Author = existingAuthor;
                    }

                    _booksService.CreateNewBook(book);
                }
                else
                {
                    existingBook.BookImage = item.BookImage;
                    existingBook.Price = item.Price;
                    existingBook.ReleaseDate = item.ReleaseDate;

                    var existingPublisher = _publisherService.GetAll().FirstOrDefault(p => p.Name == item.Publisher.Name);
                    var existingAuthor = _authorService.GetAll().FirstOrDefault(a => a.FirstName == item.Author.FirstName && a.LastName == item.Author.LastName);

                    // Check if Publisher and Author exist
                    if (existingPublisher != null)
                    {
                        existingBook.Publisher = existingPublisher;
                    }
                    else
                    {
                        existingBook.Publisher = item.Publisher;
                    }

                    if (existingAuthor != null)
                    {
                        existingBook.Author = existingAuthor;
                    }
                    else
                    {
                        existingBook.Author = item.Author;
                    }

                    _booksService.UpdateExistingBook(existingBook);
           
                }

       
            }
            return status;
        }
    }
}

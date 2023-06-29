using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NhonOJT_redis.Services;

namespace NhonOJT_redis.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public BookController(ApplicationDbContext context, IMapper mapper, ICacheService cacheService)
        {
            _context = context;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetAllAsync()
        {
            var cachedBooks = await _cacheService.Get<List<Book>>("all_books");
            if (cachedBooks != null)
            {
                return cachedBooks;
            }

            var expiryTime = DateTimeOffset.Now.AddSeconds(30);
            var books = _context.Books.ToList();
            await _cacheService.Set("all_books", books, expiryTime);

            return books;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetById(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book == null)
            {
                return NotFound("Book not found");
            }
            return book;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(BookDTO request)
        {
            var book = _mapper.Map<Book>(request);
            
            await _context.AddAsync(book);
            await _context.SaveChangesAsync();

            await _cacheService.Remove("all_books");

            return book.Id;
        }

        [HttpPut]
        public async Task<ActionResult<int>> Update(Book request)
        {
            var existingBook = await _context.Books.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id);
            if (existingBook == null)
            {
                return NotFound("Book not found");
            }

            _context.Update(request);
            await _context.SaveChangesAsync();
            await _cacheService.Remove("all_books");

            return existingBook.Id;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteAsync(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book == null)
            {
                return NotFound("Book not found");
            }

            _context.Remove(book);
            await _context.SaveChangesAsync();
            await _cacheService.Remove("all_books");

            return book.Id;
        }
    }

}
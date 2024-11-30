using Library.DbContexts;
using Library.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BooksController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult GetListView()
        {
            return View("Index", _context.Books.ToList());
        }
        public IActionResult GetAdminListView()
        {
            return View("Index - Copy", _context.Books.ToList());
        }

        public IActionResult Index(string searchString)
        {
            try
            {
                var books = _context.Books.AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    books = books.Where(x => x.Name.Contains(searchString)
                    || x.genre.Contains(searchString));
                }

                return View(books.ToList());
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        public IActionResult GetAddView()
        {

            return View("Add");
        }
        [HttpPost]
        public IActionResult AddNew(Book book)
        {
            if (_context.Books.Any(bo => bo.Name == book.Name))
            {
                ModelState.AddModelError(string.Empty, "Book is exist");
            }

            if (ModelState.IsValid == true)
            {
                    if (book.ImageFile == null)
                    {
                        book.ImagePath = "\\images\\No_Image.png";
                    }
                    else
                    {
                        Guid imgGuid = Guid.NewGuid();
                        string imgExtension = Path.GetExtension(book.ImageFile.FileName);
                        string imgPath = "\\images\\" + imgGuid + imgExtension;
                        book.ImagePath = imgPath;

                        string imgFullPath = _webHostEnvironment.WebRootPath + imgPath;

                        FileStream fileStream = new FileStream(imgFullPath, FileMode.Create);
                        book.ImageFile.CopyTo(fileStream);
                        fileStream.Dispose();
                    }
                    _context.Books.Add(book);
                    _context.SaveChanges();
                    return RedirectToAction("GetAdminListView");
            }
            else
            {
                return View("Add");
            }
        }
        [HttpGet]
        public IActionResult GetBorrowView()
		{
			return View("Borrow", _context.Books.ToList());
		}
		[HttpGet]
        public IActionResult GetDeleteView()
        {
            var books = _context.Books.ToList();

            return View("Delete", books);
        }
        [HttpGet]
        public IActionResult GetAlreadyDeleteView(int id)
        {
            Book book = _context.Books.Include(bo => bo.AccountBooks).FirstOrDefault(bo1 => bo1.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            else
            {
                return View("AlreadyDelete", book);
            }
        }

        [HttpPost]
        public IActionResult DeleteCurrent(int id)
        {
            Book book = _context.Books.Find(id);
            
            if (book == null)
            {
                return NotFound(); 
            }
        
            _context.Books.Remove(book);
            _context.SaveChanges();
        
            return RedirectToAction("GetDeleteView");
        }
    }
}

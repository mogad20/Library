using Library.DbContexts;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Hosting;

namespace Library.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public object Session { get; private set; }

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString)
        {
            try
            {
                var accounts = _context.Accounts.AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    accounts = accounts .Where (x => x.FirstName.Contains(searchString)
                    || x.LastName.Contains(searchString)
                    || x.Address.Contains(searchString));
                }

                return View(accounts.ToList());
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        public IActionResult GetIndexView()
        {
            return View("Index" , _context.Accounts.ToList());
        }

        public IActionResult GetAccView()
        {
            return View("Index", _context.Accounts.ToList());
        }

        [HttpGet]
        public IActionResult GetRegisterView()
        {
            ViewBag.AllAccounts = _context.Accounts.ToList();
            return View("Register");
        }
        public IActionResult GetLoginView()
        {
            return View("Login");
        }

        [HttpPost]
        public IActionResult AddNew(Account account)
        {
            if (_context.Accounts.Any(acc => acc.Email == account.Email))
            {
                ModelState.AddModelError(string.Empty, "Duplicated Email");
            }


            if (ModelState.IsValid == true)
            {
                _context.Accounts.Add(account);
                _context.SaveChanges();
                return RedirectToAction("GetLoginView");
            }
            else
            {
                return View("Register");
            }
        }

        [HttpPost]
        public IActionResult Log_in(Logging logging)
        {
            if (ModelState.IsValid == true)
            {
                var user = _context.Accounts.SingleOrDefault(acc => acc.Email == logging.Email);
                ViewBag.userEmail = user;
                if (user != null && VerifyPassword(logging.Password, user.Password))
                {
                    return RedirectToAction("GetListView" , "Books");
                }    
            }
            
            return RedirectToAction("GetRegisterView" , "Accounts");

        }
        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            return enteredPassword == storedPassword;
        }

        [HttpPost]
        public IActionResult EditCurrent(Account account)
		{
			if (_context.Accounts.Any(acc => acc.Email == account.Email && acc.Id != account.Id))
			{
				ModelState.AddModelError(string.Empty, "Email is dublicate");
			}

			if (ModelState.IsValid == true)
			{
				_context.Accounts.Update(account);
				_context.SaveChanges();
				return RedirectToAction("GetAccView");
			}
			else
			{
				return View("Edit");
			}
		}
        [HttpGet]
        public IActionResult GetEditView(int Id)
        {
            Account account = _context.Accounts.Find(Id);
            if (account == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.AllDepartments = _context.Accounts.ToList();
                return View("Edit", account);
            }
        }

        [HttpPost]
        public IActionResult EditUserCurrent(Account account)
        {
            if (_context.Accounts.Any(acc => acc.Email == account.Email && acc.Id != account.Id))
            {
                ModelState.AddModelError(string.Empty, "Email is dublicate");
            }

            if (ModelState.IsValid == true)
            {
                _context.Accounts.Update(account);
                _context.SaveChanges();
                return RedirectToAction("GetAdminListView");
            }
            else
            {
                return View("UserEdit");
            }
        }
        [HttpGet]
        public IActionResult GetEditUserView(int Id)
        {
            Account account = _context.Accounts.Find(Id);
            if (account == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.AllDepartments = _context.Accounts.ToList();
                return View("UserEdit", account);
            }
        }

        [HttpGet]
        public IActionResult GetDeleteView(int id)
        {
            Account account = _context.Accounts.Include(acc => acc.AccountBooks).FirstOrDefault(emp1 => emp1.Id == id);
            if (account == null)
            {
                return NotFound();
            }
            else
            {
                return View("Delete", account);
            }
        }

        [HttpPost]
        public IActionResult DeleteCurrent(int id)
        {
            Account account = _context.Accounts.Find(id);

            _context.Accounts.Remove(account);
            _context.SaveChanges();
            return RedirectToAction("GetAccView");
        }

    }
}

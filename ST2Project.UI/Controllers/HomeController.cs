using Microsoft.AspNetCore.Mvc;
using ST2Project.UI.Models;
using ST2Project.Logic.Services;
using System.Diagnostics;
using ST2Project.Logic.Entities;
using ST2Project.UI.Validators;
using ST2Project.UI.Session;
using FluentValidation;

namespace ST2Project.UI.Controllers
{
    public class HomeController : Controller
    {     
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("Login");
        }

        public IActionResult Register()
        {
            return View("Registration");
        }

        [HttpPost]
        public IActionResult RegisterUser(UserModel _model)
        {
            UserValidator _val = new UserValidator();
            var results = _val.Validate(_model);
            if (!results.IsValid)
            {
                return View("Registration");           
            }
            Users tmp = new Users()
            {
                FirstName = _model.FirstName,
                LastName = _model.LastName,
                Username = _model.Username,
                Email = _model.Email,
                Password = _model.Password,
            };
            new UserService().RegisterUser(tmp);
            return View("Login");
        }

        public IActionResult LoginUser(UserModel _model)
        {
            if(new UserService().IsUsersDataCorrect(new Users() { Username = _model.Username, Password = _model.Password })){      
                var tmp = new UserService().FetchUserData(new Users() { Username = _model.Username, Password = _model.Password });
                var _loggedUser = new UserModel()
                {
                    Id = tmp.UserID,
                    FirstName = tmp.FirstName,
                    LastName = tmp.LastName,
                    Username= tmp.Username,
                    Email = tmp.Email,
                    Password = tmp.Password,
                };
                SessionService.SaveSession(_loggedUser);
                SessionService._isLoggedIn = true;
                return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.Error = "You passed wrong username/password";
            return View("Login");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
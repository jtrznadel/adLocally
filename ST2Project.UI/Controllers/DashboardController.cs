using Microsoft.AspNetCore.Mvc;
using ST2Project.UI.Models;
using ST2Project.Logic.Services;
using ST2Project.Logic.Entities;
using ST2Project.UI.Validators;
using ST2Project.UI.Session;
using CsvHelper;
using System.Text;

namespace ST2Project.UI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserModel _user = SessionService._currentUser;
        private readonly bool _isAdmin = SessionService._isAdmin;
        public IActionResult Index(string searchString)
        {         
            List<OfferModel> _list = new List<OfferModel>();
            if (!String.IsNullOrEmpty(searchString))
            {
                foreach (var offer in new OfferService().GetSpecifiedOffers(searchString))
                {
                    _list.Add(new OfferModel()
                    {
                        OfferId = offer.OfferID,
                        Title = offer.Title,
                        Description = offer.Description,
                        Price = offer.Price,
                        Condition = offer.Condition,
                        Location = offer.Location,
                    });
                }
            }
            else
            {
                foreach (var offer in new OfferService().GetAllOffers())
                {
                    _list.Add(new OfferModel()
                    {
                        OfferId = offer.OfferID,
                        Title = offer.Title,
                        Description = offer.Description,
                        Price = offer.Price,
                        Condition = offer.Condition,
                        Location = offer.Location,
                    });
                }
            }
            ViewBag.IsOwn = false;
            ViewBag.IsAdmin = _isAdmin;
            ViewData["Offers"] = _list;
            ViewData["UserData"] = $"{_user.FirstName} {_user.LastName}";
            return View("Panel");
        }

        public IActionResult CreateOffer()
        {
            var item = new OfferModel();
            return View("CreateOffer", item);
        }

        public IActionResult ShowUserOffers(string searchString)
        {
            List<OfferModel> _list = new List<OfferModel>();
            if (!String.IsNullOrEmpty(searchString))
            {
                foreach (var offer in new OfferService().GetSpecifiedOffers(searchString))
                {
                    _list.Add(new OfferModel()
                    {
                        OfferId = offer.OfferID,
                        Title = offer.Title,
                        Description = offer.Description,
                        Price = offer.Price,
                        Condition = offer.Condition,
                        Location = offer.Location,
                    });
                }
            }
            else
            {
                foreach (var offer in new OfferService().GetUsersOffers(_user.Id))
                {
                    _list.Add(new OfferModel()
                    {
                        OfferId = offer.OfferID,
                        Title = offer.Title,
                        Description = offer.Description,
                        Price = offer.Price,
                        Condition = offer.Condition,
                        Location = offer.Location,
                    });
                }
            }
            ViewBag.IsOwn = true;
            ViewBag.IsAdmin = _isAdmin;
            ViewData["Offers"] = _list;
            ViewData["UserData"] = $"{_user.FirstName} {_user.LastName}";
            return View("Panel");
        }

        public IActionResult AddOffer(OfferModel _model)
        {
            TempData["Alert"] = "";
            OfferValidator  _val = new OfferValidator();
            var results = _val.Validate(_model);
            var item = new OfferModel();
            if (!results.IsValid)
            {
                TempData["Alert"] = "Something went wrong. Offer cannot be added";           
                ModelState.Clear();
                return View("CreateOffer", item);
            }
            Offers tmp = new Offers()
            {
                Title = _model.Title,
                Description = _model.Description,
                Price = _model.Price,
                Condition = _model.Condition,
                Location = _model.Location,
                OwnerID = _user.Id
            };
            new OfferService().CreateOffer(tmp);
            TempData["Alert"] = "Offer successfuly added";
            ModelState.Clear();
            return View("CreateOffer", item);
        }

        public IActionResult RemoveOffer(int value)
        {
            new OfferService().RemoveOffer(value);
            if(_isAdmin) return RedirectToAction("Index");
            else return RedirectToAction("ShowUserOffers");
        }

        public IActionResult EditOffer(int value)
        {
            TempData["Alert"] = "";
            var offerToEdit = new OfferService().GetSingleOffer(value);
            var _model = new OfferModel()
            {
                OfferId = offerToEdit.OfferID,
                Title = offerToEdit.Title,
                Description = offerToEdit.Description,
                Price = offerToEdit.Price,
                Location = offerToEdit.Location,
                Condition = offerToEdit.Condition
            };
            return View("EditOffer", _model);
        }

        public IActionResult EditOfferExecute(OfferModel _model)
        {
            TempData["Alert"] = "";
            OfferValidator _val = new OfferValidator();
            var results = _val.Validate(_model);
            var item = new OfferModel();
            if (!results.IsValid)
            {
                TempData["Alert"] = "Something went wrong. Offer cannot be edited";
                ModelState.Clear();
                RedirectToAction("ShowUserOffers");
            }
            Offers tmp = new Offers()
            {
                OfferID = _model.OfferId,
                Title = _model.Title,
                Description = _model.Description,
                Price = _model.Price,
                Condition = _model.Condition,
                Location = _model.Location,
                OwnerID = _user.Id
            };
            new OfferService().EditOffer(tmp);
            TempData["Alert"] = "Offer successfuly edited";
            ModelState.Clear();
            return RedirectToAction("ShowUserOffers");
        }
        public FileContentResult SaveToFile()
        {
            List<OfferModel> _list = new List<OfferModel>();
            foreach (var offer in new OfferService().GetAllOffers())
            {
                _list.Add(new OfferModel()
                {
                    Title = offer.Title,
                    Description = offer.Description,
                    Price = offer.Price,
                    Condition = offer.Condition,
                    Location = offer.Location,
                });
            }
            string csv = ListToCSV(_list);

            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "report.csv");
        }
        private string ListToCSV<T>(IEnumerable<T> list)
        {
            StringBuilder sList = new StringBuilder();

            Type type = typeof(T);
            var props = type.GetProperties();
            sList.Append(string.Join(",", props.Select(p => p.Name)));
            sList.Append(Environment.NewLine);

            foreach (var element in list)
            {
                sList.Append(string.Join(",", props.Select(p => p.GetValue(element, null))));
                sList.Append(Environment.NewLine);
            }

            return sList.ToString();
        }

        public IActionResult RemoveUsers()
        {
            List<UserModel> _list = new List<UserModel>();
            foreach (var offer in new UserService().GetAllUsers())
            {
                _list.Add(new UserModel()
                {
                    Id = offer.UserID,
                    LastName = offer.LastName,
                    Email = offer.Email,
                    FirstName = offer.FirstName,
                    Password = offer.Password,
                    Username = offer.Username,
                });
            }
            ViewBag.IsAdmin = _isAdmin;
            ViewData["Users"] = _list;
            ViewData["UserData"] = $"{_user.FirstName} {_user.LastName}";
            return View("RemoveUsers");
        }

        public IActionResult RemoveUserExecute(int value)
        {
            new UserService().RemoveUser(value);
            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            SessionService.CloseSession();
            return RedirectToAction("Index", "Home");
        }
    }
}


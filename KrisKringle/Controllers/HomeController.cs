using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using KrisKringle.Models;
using KrisKringle.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PRGPera.Web.Models.Ddr;

namespace KrisKringle.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Kris_dbContext _dbContext;
        public readonly IKrisKringleService _krisKringleService;
        CipherDecipher cipher = new CipherDecipher();
        public HomeController(ILogger<HomeController> logger, Kris_dbContext dbContext, IKrisKringleService krisKringleService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _krisKringleService = krisKringleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(Usr user)
        {
            if ((user.Username == null && user.Password == null) || (user.Username != null && user.Password == null) || (user.Username == null && user.Password != null))
            {
                return PartialView("_popMsgUsr");
            }
            else
            {
                var uname = cipher.Encrypt(user.Username.ToString());
                var pass = cipher.Encrypt(user.Password.ToString());
                var username = _krisKringleService.GetUsers().Where(x => x.Username == uname && x.IsAdmin == false).Select(x => x.Username).FirstOrDefault();
                var password = _krisKringleService.GetUsers().Where(x => x.Password == pass && x.IsAdmin == false).Select(x => x.Password).FirstOrDefault();
                var usernameAdmin = _krisKringleService.GetUsers().Where(x => x.Username == uname && x.IsAdmin == true).Select(x => x.Username).FirstOrDefault();
                var passwordAdmin = _krisKringleService.GetUsers().Where(x => x.Password == pass && x.IsAdmin == true).Select(x => x.Password).FirstOrDefault();               

                if ((username != null || password != null) && (username != null || password == null) && (username == null || password != null))
                {
                    var deUsername = cipher.Decrypt(username.ToString());
                    var dePassword = cipher.Decrypt(password.ToString());
                    if (user.Username == deUsername && user.Password == dePassword)
                    {
                        var userid = _krisKringleService.GetUsers().Where(x => x.Username == username.ToString()).Select(x => x.Id).FirstOrDefault();
                        var wishs = _krisKringleService.GetWish().Where(x => x.UsersId == userid).Select(x => x).ToList().Count();
                        if(wishs != 0)
                        {
                            return Redirect("/KrisKringle/Index/" + uname);
                        }
                        else
                        {
                            return Redirect("/KrisKringle/WishPage/" + uname);
                        }                       
                    }
                    else
                    {
                        return PartialView("_popMsgUsr");
                    }
                }
                if ((usernameAdmin != null || passwordAdmin != null) && (usernameAdmin != null || passwordAdmin == null) && (usernameAdmin == null || passwordAdmin != null))
                {
                    var deusernameAdmin = cipher.Decrypt(usernameAdmin.ToString());
                    var depasswordAdmin = cipher.Decrypt(passwordAdmin.ToString());
                    if (user.Username == deusernameAdmin && user.Password == depasswordAdmin)
                    {
                        var userid = _krisKringleService.GetUsers().Where(x => x.Username == usernameAdmin.ToString()).Select(x => x.Id).FirstOrDefault();
                        var wishs = _krisKringleService.GetWish().Where(x => x.UsersId == userid).Select(x => x).ToList().Count();
                        if (wishs != 0)
                        {
                            return Redirect("/KrisKringle/Admin/" + uname);
                        }
                        else
                        {
                            return Redirect("/KrisKringle/WishPage/" + uname);
                        }
                    }
                    else
                    {
                        return PartialView("_popMsgUsr");
                    }
                }
                else
                {
                    return PartialView("_popMsgUsr");
                }
            }
        }
        public void GetDepartmentList() //It will be fired from Jquery ajax call
        {
            var deptList = _krisKringleService.GetDepartmentDD();
            deptList.Insert(0, new SelectListItem()
            {
                Text = "---Select---",
                Value = string.Empty
            });
            ViewBag.deplist = deptList;
        }

        public IActionResult Users(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            GetDepartmentList();
            var uid = Url.ActionContext.RouteData.Values["id"];
            var id = Convert.ToInt32(uid);
            var username= _krisKringleService.GetUsers().Where(x => x.Id == id).Select(x => x.Username).FirstOrDefault();
            var nickname = _krisKringleService.GetUsers().Where(x => x.Id == id).Select(x => x.Nick).FirstOrDefault();
            var admin = _krisKringleService.GetUsers().Where(x => x.Id == id).Select(x => x.IsAdmin).FirstOrDefault();
            var password = _krisKringleService.GetUsers().Where(x => x.Id == id).Select(x => x.Password).FirstOrDefault();
            if(username != null || password != null)
            {
                ViewData["username"] = cipher.Decrypt(username.ToString());
                ViewData["password"] = cipher.Decrypt(password.ToString());
            }
            ViewData["nick"] = nickname;
            ViewData["id"] = id;
            ViewData["admin"] = admin;
            
            if (admin == true || uid == null)
            {
                ViewBag.IsAdmin = "null";
            }
            var usrs = new List<Usr>();
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
           
            var items = _krisKringleService.GetUsers().ToList();
            foreach (var item in items)
            {
                var deptName = _krisKringleService.GetDepartment().Where(x => x.Id == item.DepartmentId.GetValueOrDefault()).Select(x => x.Name).FirstOrDefault();
                var model = new Usr();
                model.Id = item.Id;
                model.Username = cipher.Decrypt(item.Username);
                model.Nick = item.Nick;
                model.DepartmentName = deptName;
                model.IsAdmin = (bool)item.IsAdmin;
                if(item.DepartmentId != null)
                {
                    model.DepartmentId = (int)item.DepartmentId;
                    ViewData["DepartmentId"] = model.DepartmentId;
                }
                usrs.Add(model);               
            }
            usrs = usrs.OrderBy(x => x.Username).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper();
                usrs = usrs.Where(x => x.Username.ToUpper().Contains(searchString)).ToList();
            }
            switch (sortOrder)
            {
                case "name_desc":
                    usrs = usrs.OrderByDescending(s => s.Username).ToList();
                    break;
                default:
                    usrs = usrs.OrderBy(s => s.Nick).ToList();
                    break;
            }
            int pageSize = 5;

            var ret = PaginatedList<Usr>.Create(usrs.AsQueryable(), pageNumber ?? 1, pageSize);
            return View(ret);
        }
        [HttpPost]
        public IActionResult AddUpdateUser(string username,string password, string nick, string deptId, string id, string admin)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Error occurred please see inputs");
                return Json(new { isValid = false, modelState = ModelState.Values });
            }
            var enUsername = cipher.Encrypt(username);
            var enPassword = cipher.Encrypt(password);
            var data = _krisKringleService.GetUsers().Where(x => x.Id == Convert.ToInt32(id)).FirstOrDefault();
            if (id == "0")
            {
                var model = new Users();
                model.Username = enUsername;
                model.Password = enPassword;
                model.Nick = nick;
                model.IsAdmin = Convert.ToBoolean(admin);
                model.DepartmentId = Convert.ToInt32(deptId);
               
                _krisKringleService.AddUpdateUser(model);

                return Json(new { isValid = true, redirect = Url.Action("Users", "Home") });
            }
            else if (id == data.Id.ToString())
            {
                var model = new Users();
                model.Id = data.Id;
                model.Username = enUsername;
                model.Password = enPassword;
                model.Nick = nick;
                model.IsAdmin = Convert.ToBoolean(admin);
                model.DepartmentId = Convert.ToInt32(deptId);

                _krisKringleService.AddUpdateUser(model);

                return Json(new { isValid = true, redirect = Url.Action("Users", "Home") });
            }
            else
            {
                return Json(new { err = 1, error = "User already existed" });
            }
        }
        public IActionResult Department(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            var uid = Url.ActionContext.RouteData.Values["id"];
            var id = Convert.ToInt32(uid);
            var dept = _krisKringleService.GetDepartment().Where(x=>x.Id == id).Select(x => x.Name).FirstOrDefault();
            ViewData["dept"] = dept;
            ViewData["id"] = id;
            var active = _krisKringleService.GetDepartment().Where(x => x.Id == id).Select(x => x.Active).FirstOrDefault();
            if(active == true || uid == null)
            {
                ViewBag.Active = "null";
            }
            var depatment = new List<Department>();
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var items = _krisKringleService.GetDepartment().ToList();
            foreach (var item in items)
            {
                var model = new Department();
                model.Id = item.Id;
                model.Name = item.Name;
                model.Active = item.Active;
                depatment.Add(model);
            }
            depatment = depatment.OrderBy(x => x.Name).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper();
                depatment = depatment.Where(x => x.Name.ToUpper().Contains(searchString)).ToList();
            }
            switch (sortOrder)
            {
                case "name_desc":
                    depatment = depatment.OrderByDescending(s => s.Name).ToList();
                    break;
                default:
                    depatment = depatment.OrderBy(s => s.Active).ToList();
                    break;
            }
            int pageSize = 3;

            var ret = PaginatedList<Department>.Create(depatment.AsQueryable(), pageNumber ?? 1, pageSize);
            return View(ret);
        }
        [HttpPost]
        public IActionResult AddUpdateDepartment(string department, string active, string id)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Error occurred please see inputs");
                return Json(new { isValid = false, modelState = ModelState.Values });
            }
            var data = _krisKringleService.GetDepartment().Where(x => x.Name == department).FirstOrDefault();
            if (id == "0")
            {
                var model = new Department();
                model.Name = department;
                model.Active = true;
                _krisKringleService.AddUpdateDepartment(model);

                return Json(new { isValid = true, redirect = Url.Action("Department", "Home") });
            }
            else if(id == data.Id.ToString())
            {
                var model = new Department();
                model.Id = data.Id;
                model.Name = department;
                model.Active = Convert.ToBoolean(active);
                _krisKringleService.AddUpdateDepartment(model);

                return Json(new { isValid = true, redirect = Url.Action("Department", "Home") });
            }
            else
            {
                return Json(new { err = 1, error = "Department already existed" });
            }
        }

        public IActionResult ForgetPassword(string email,string uname)
        {
            try
            {
                string emailBody = string.Empty, ret = string.Empty, FirstName = string.Empty;
                if (email == null || uname == null)
                {
                    ret = "null";
                    return Json(ret);
                }
                var enUname = cipher.Encrypt(uname.ToString());
                var password = _krisKringleService.GetUsers().Where(x => x.Username == enUname).Select(x => x.Password).FirstOrDefault();
                var dePassword = cipher.Decrypt(password.ToString());               

                emailBody = "<h5>Hi " + uname + " </h5>" +
                              "PASSWORD:   " + dePassword + "</br>";
                EmailModel emodel = new EmailModel
                {
                    From = "ict.systems@princeretail.com",
                    Port = 587,
                    Host = "smtp.office365.com",
                    To = email,
                    BCC = "jcabili@princeretail.com",
                    //CC = //"cfrancis@princeretail.com" + "," + "ealiasot@princeretail.com" + "," + "jcabili@princeretail.com",
                    Subject = "PASSWORD RECOVERY",
                    Body = emailBody,
                    Password = "@PR1nc3$$$",
                };
                emodel.ENotify(emodel);
                ret = "Check email";
                return Json(ret);
            }
           catch(Exception ex)
            {
                var ret = (ex.Message);
                return Json(ret);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KrisKringle.Models;
using KrisKringle.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRGPera.Web.Models.Ddr;

namespace KrisKringle.Controllers
{
    public class KrisKringleController : Controller
    {
        CipherDecipher cipher = new CipherDecipher();
        private readonly Kris_dbContext _dbContext;
        public readonly IKrisKringleService _krisKringleService;

        public KrisKringleController(Kris_dbContext dbContext, IKrisKringleService krisKringleService)
        {
            _dbContext = dbContext;
            _krisKringleService = krisKringleService;
        }
        public IActionResult Index()
        {
            var uid = Url.ActionContext.RouteData.Values["id"];
            var id = cipher.Decrypt(uid.ToString());
            var userid = _krisKringleService.GetUsers().Where(x => x.Username == uid.ToString()).Select(x => x.Id).FirstOrDefault();
            var wishCount= _krisKringleService.GetWish().Where(x => x.UsersId == userid && x.Wish1 != null).Select(x => x.Id).ToList().Count();
            var usersId = _krisKringleService.GetUsers().Where(x => x.Id == userid).Select(x => x.Id).FirstOrDefault();

            var users = new List<UserWish>();           
            var data = _krisKringleService.GetUsers();
            foreach (var item in data)
            {
                var nick = _krisKringleService.GetUsers().Where(x => x.Id == item.Id).Select(x => x.Nick).FirstOrDefault();
                var deusername = nick;
                var cmtns = _krisKringleService.GetComments().Where(x => x.UsersId == item.Id).Select(x => x).ToList();
                var wishs = _krisKringleService.GetWish().Where(x => x.UsersId == item.Id).Select(x => x).ToList();
                var wishUser = _krisKringleService.GetWish().Where(x => x.UsersId == item.Id).Select(x => x.Wish1).FirstOrDefault();
                var datesUser = _krisKringleService.GetWish().Where(x => x.UsersId == item.Id).Select(x => x.WishTime).FirstOrDefault();
                var model = new UserWish();
                model.Id = item.Id;
                model.UsersId = userid;
                model.Nick = item.Nick;
                model.comments = cmtns;
                model.wishes = wishs;
                model.Username = deusername;
                if(usersId == item.Id)
                {
                    model.UsrBtn = 1;
                }
                else
                {
                    model.UsrBtn = 0;
                }
                if (datesUser != null)
                {
                    model.WishTime = datesUser;
                }
                if (wishUser != null)
                {
                    model.Wish1 = wishUser;
                }
                users.Add(model);
                ViewBag.UsrBtn = model.UsrBtn;
                ViewData["UsersId"] = model.UsersId;
            }
            return View(users.OrderBy(x => x.Id == userid ? 0 : 1).ThenBy(x =>x.Nick).ToList());
        }
        public IActionResult Admin()
        {
            var uid = Url.ActionContext.RouteData.Values["id"];
            var id = cipher.Decrypt(uid.ToString());
            var userid = _krisKringleService.GetUsers().Where(x => x.Username == uid.ToString()).Select(x => x.Id).FirstOrDefault();
            var wishCount = _krisKringleService.GetWish().Where(x => x.UsersId == userid && x.Wish1 != null).Select(x => x.Id).ToList().Count();
            var usersId = _krisKringleService.GetUsers().Where(x => x.Id == userid).Select(x => x.Id).FirstOrDefault();
            ViewData["uid"] = uid;

            var users = new List<UserWish>();
            var data = _krisKringleService.GetUsers();
            foreach (var item in data)
            {
                var nick = _krisKringleService.GetUsers().Where(x => x.Id == item.Id).Select(x => x.Nick).FirstOrDefault();
                var deusername = nick;
                var cmtns = _krisKringleService.GetComments().Where(x => x.UsersId == item.Id).Select(x => x).ToList();
                var wishs = _krisKringleService.GetWish().Where(x => x.UsersId == item.Id).Select(x => x).ToList();
                var wishUser = _krisKringleService.GetWish().Where(x => x.UsersId == item.Id).Select(x => x.Wish1).FirstOrDefault();
                var datesUser = _krisKringleService.GetWish().Where(x => x.UsersId == item.Id).Select(x => x.WishTime).FirstOrDefault();
                var model = new UserWish();
                model.Id = item.Id;
                model.UsersId = userid;
                model.Nick = item.Nick;
                model.comments = cmtns;
                model.wishes = wishs;
                model.Username = deusername;
                if (usersId == item.Id)
                {
                    model.UsrBtn = 1;
                }
                else
                {
                    model.UsrBtn = 0;
                }
                if (datesUser != null)
                {
                    model.WishTime = datesUser;
                }
                if (wishUser != null)
                {
                    model.Wish1 = wishUser;
                }
                users.Add(model);
                ViewBag.UsrBtn = model.UsrBtn;
                ViewData["UsersId"] = model.UsersId;
            }
            return View(users.OrderBy(x => x.Id == userid ? 0 : 1).ThenBy(x => x.Nick).ToList());
        }

        public IActionResult redIrect(string user)
        {           
            return Redirect("/KrisKringle/Index/" + user);
        }
        [HttpPost]
        //public IActionResult AddWish(List<string> dates, List<string> wishes, string id)
        public IActionResult UpdateWish(string wish1, string wish2, string wish3, string userid)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Error occurred please see inputs");
                return Json(new { isValid = false, modelState = ModelState.Values });
            }
            var dtime = DateTime.Now;
            var usersid = Convert.ToInt32(userid);
            var username = _krisKringleService.GetUsers().Where(x => x.Id == usersid).Select(x => x.Username).FirstOrDefault();
            
            var model = new UserWish();           
            var wishCount = _dbContext.Wish.Where(d => d.UsersId == usersid).Select(x => x.Id).ToList().Count();
            if (wishCount != 0 && userid != null)
            {
                model.UsersId = usersid;

                _krisKringleService.DeleteWish(model);
                for (int x = 1; x <= 3; x++)
                {
                    if (x == 1)
                    {
                        model.Wish1 = wish1;
                        model.WishTime = DateTime.Now;
                        model.UsersId = usersid;
                        _krisKringleService.AddWish(model);
                    }
                    if (x == 2)
                    {
                        model.Wish1 = wish2;
                        model.WishTime = DateTime.Now;
                        model.UsersId = usersid;
                        _krisKringleService.AddWish(model);
                    }
                    if (x == 3)
                    {
                        model.Wish1 = wish3;
                        model.WishTime = DateTime.Now;
                        model.UsersId = usersid;
                        _krisKringleService.AddWish(model);
                    }
                }
                return Json(new { isValid = true, username = username });
            }
            var ret = 1;
            return Json(ret);
        }

        public IActionResult ViewResult(string id)
        {
            var wish = _krisKringleService.GetWish().Where(x=>x.UsersId == Convert.ToInt32(id)).ToList();
            var uwish = new List<UserWish>();
            foreach (var item in wish)
            {
                var date = _krisKringleService.GetDates().Where(x => x.Id == item.Id).Select(x => x.Dates).FirstOrDefault();
                var model = new UserWish();
                model.Id = item.Id;
                model.Wish1 = item.Wish1;
                model.Dates = date;
                uwish.Add(model);
            }
            return View(uwish);
        }

        public IActionResult Wish(string id)
        {
            var item = new List<Wishes>();
            var model = new Wishes();
            var userid = _krisKringleService.GetUsers().Where(x => x.Id == Convert.ToInt32(id)).Select(x => x.Id).FirstOrDefault();
            var wishes = _krisKringleService.GetWish().Where(x => x.UsersId== userid).ToList();
            foreach(var wish in wishes)
            {               
                model.UsersId = wish.UsersId;
                model.Wish1 = wishes[0].Wish1;
                model.Wish2 = wishes[1].Wish1;
                model.Wish3 = wishes[2].Wish1;
                item.Add(model);
            }           
            return View(model);
        }
        public IActionResult Comment(string id, string userCommentId)
        {
            var item = new List<UserWish>();
            var model = new UserWish();
            var userid = _krisKringleService.GetUsers().Where(x => x.Id == Convert.ToInt32(id)).Select(x => x.Id).FirstOrDefault();
            var commntUser = _krisKringleService.GetUsers().Where(x => x.Id == Convert.ToInt32(userCommentId)).Select(x => x.Nick).FirstOrDefault();
            model.UsersId = userid;
            model.UserComment = commntUser;
            item.Add(model);
            return View(model);
        }
        [HttpPost]
        public IActionResult AddComment(string userid, string note,string commentUser)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Error occurred please see inputs");
                return Json(new { isValid = false, modelState = ModelState.Values });
            }
            //var datesWish = wishes.Zip(dates, (t, d) => new { wis = t, dte = d });
            //foreach(var dw in datesWish)
            //{
            //    var item = new Wish();
            //    item.Wish1 = dw.wis;
            //    item.UsersId = Convert.ToInt32(id);
            //}
            //return Json(id);
            var dtime = DateTime.Now;
            var usersid = Convert.ToInt32(userid);
            var username = _krisKringleService.GetUsers().Where(x => x.Id == usersid).Select(x => x.Username).FirstOrDefault();
            //var nickUsrCommnt = _krisKringleService.GetUsers().Where(x => x.Id == UsrCommntId).Select(x => x.Nick).FirstOrDefault();
            //var CommentId = _krisKringleService.GetComments().Where(x => x.UsersId == usersid).Select(x => x.Id).FirstOrDefault();
            var item = new UserWish()
            {
                Notes = note,
                UsersId = usersid,
                WishTime = dtime,
                UserComment = commentUser,

            };
            _krisKringleService.AddComment(item);
            return Json(new { username = username });
        }

        public IActionResult ChangePass(string userid)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Error occurred please see inputs");
                return Json(new { isValid = false, modelState = ModelState.Values });
            }
            var usersid = Convert.ToInt32(userid);
            var username = _krisKringleService.GetUsers().Where(x => x.Id == usersid).Select(x => x.Username).FirstOrDefault();
            var password = _krisKringleService.GetUsers().Where(x => x.Id == usersid).Select(x => x.Password).FirstOrDefault();
            var deUsernamme = cipher.Decrypt(username);
            var dePassword = cipher.Decrypt(password);
            ViewData["username"] = deUsernamme;
            ViewData["password"] = dePassword;
            ViewData["usersid"] = usersid;

            return Json(new { isValid = true, deUsernamme = deUsernamme, dePassword = dePassword, usersid = usersid });
        }

        public IActionResult UpdatePassUser(string userid, string username, string password)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Error occurred please see inputs");
                return Json(new { isValid = false, modelState = ModelState.Values });
            }
            if(username == null || password == null)
            {
                var ret = 1;
                return Json(ret);
            }
            var data = _krisKringleService.GetUsers().Where(x => x.Id == Convert.ToInt32(userid)).FirstOrDefault();
            var enUsername = cipher.Encrypt(username);
            var enPassword = cipher.Encrypt(password);

            var model = new Users();
            model.Id = Convert.ToInt32(userid);
            model.Username = enUsername;
            model.Password = enPassword;
            model.Nick = data.Nick;
            model.IsAdmin = data.IsAdmin;
            model.DepartmentId = data.DepartmentId;

            _krisKringleService.AddUpdateUser(model);
            return Json(new { isValid = true, enUsername = enUsername });
        }

        public IActionResult WishPage()
        {
            var uid = Url.ActionContext.RouteData.Values["id"];
            var userid = uid.ToString();
            var deUserid = cipher.Decrypt(userid);
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Error occurred please see inputs");
                return Json(new { isValid = false, modelState = ModelState.Values });
            }
            var model = new Wishes();
            model.Username = deUserid;
            return View(model);
        }
        public IActionResult AddWishes(string wish1, string wish2, string wish3, string userid)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Error occurred please see inputs");
                return Json(new { isValid = false, modelState = ModelState.Values });
            }
            var enId = cipher.Encrypt(userid);
            var uid = _krisKringleService.GetUsers().Where(x => x.Username == enId).Select(x => x.Id).FirstOrDefault();
            var model = new UserWish();
            if((wish1 == null) ||(wish2 == null) || (wish3 == null))
            {
                var ret = 1;
                return Json(ret); 
            }
            for (int x = 1;x <= 3; x++)
            {
                if(x == 1)
                {
                    model.Wish1 = wish1;
                    model.WishTime = DateTime.Now;
                    model.UsersId = uid;
                    _krisKringleService.AddWish(model);
                }
                if (x == 2)
                {
                    model.Wish1 = wish2;
                    model.WishTime = DateTime.Now;
                    model.UsersId = uid;
                    _krisKringleService.AddWish(model);
                }
                if (x == 3)
                {
                    model.Wish1 = wish3;
                    model.WishTime = DateTime.Now;
                    model.UsersId = uid;
                    _krisKringleService.AddWish(model);
                }
            }
            var url = "/KrisKringle/Index/" + enId;
            return Json(new { isValid = true, url = url });
        }
    }
}

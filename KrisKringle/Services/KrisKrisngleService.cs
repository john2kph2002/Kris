using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KrisKringle.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KrisKringle.Services
{
    public class KrisKrisngleService : IKrisKringleService
    {
        private readonly Kris_dbContext _dbContext;
        public KrisKrisngleService(Kris_dbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Comment> GetComments()
        {
            var items = new List<Comment>();
            items = _dbContext.Comment.Select(x => x).ToList();
            return items;
        }

        public List<Users> GetUsers()
        {
            var items = new List<Users>();
            items = _dbContext.Users.Select(x => x).ToList();
            return items;
        }

        public List<Department> GetDepartment()
        {
            var items = new List<Department>();
            items = _dbContext.Department.Select(x => x).ToList();
            return items;
        }

        public void AddUpdateDepartment(Department dept)
        {
            try
            {
                if (dept.Id == 0)
                {
                    var department = new Department()
                    {
                        Name = dept.Name,
                        Active = dept.Active
                    };
                    _dbContext.Department.Add(department);
                    _dbContext.SaveChanges();
                }
                else
                {
                    Department c = (from x in _dbContext.Department
                                    where x.Id == dept.Id
                                    select x).First();
                    c.Name = dept.Name;
                    c.Active = dept.Active;
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }
        }

        public void AddUpdateUser(Users user)
        {
            try
            {
                if (user.Id == 0)
                {
                    var usr = new Users()
                    {
                        Username = user.Username,
                        Password = user.Password,
                        Nick = user.Nick,
                        IsAdmin = user.IsAdmin,
                        DepartmentId = user.DepartmentId
                    };
                    _dbContext.Users.Add(usr);
                    _dbContext.SaveChanges();
                }
                else
                {
                    Users c = (from x in _dbContext.Users
                               where x.Id == user.Id
                               select x).First();
                    c.Username = user.Username;
                    c.Password = user.Password;
                    c.Nick = user.Nick;
                    c.IsAdmin = user.IsAdmin;
                    c.DepartmentId = user.DepartmentId;
                    _dbContext.SaveChanges();
                }
            }
            catch(Exception e)
            {
                
            }
        }

        public List<SelectListItem> GetDepartmentDD()
        {
            var model = new List<SelectListItem>();
            var deplist = _dbContext.Department.Select(x => x).ToList();
            foreach (var menu in deplist)
            {
                model.Add(new SelectListItem
                {
                    Text = menu.Name.ToString(),
                    Value = menu.Id.ToString(),
                });
            }
            return model.OrderBy(x => x.Text).ToList();
        }
        public List<WishDates> GetDates()
        {
            var items = new List<WishDates>();
            items = _dbContext.WishDates.Select(x => x).ToList();
            return items;
        }

        public void AddWish(UserWish wish)
        {
            var item = new Wish()
            {
                Wish1 = wish.Wish1,
                WishTime = wish.WishTime,
                UsersId = wish.UsersId
            };
            _dbContext.Wish.Add(item);
            _dbContext.SaveChanges();
        }

        public void DeleteWish(UserWish wish)
        {
            var c = _dbContext.Wish.Where(d => d.UsersId == wish.UsersId).ToList();
            _dbContext.Wish.RemoveRange(c);
            _dbContext.SaveChanges();
        }

        public void UpdateWish(UserWish wish)
        {
            var item = new Wish()
            {
                Wish1 = wish.Wish1,
                WishTime = wish.WishTime,
                UsersId = wish.UsersId
            };
            _dbContext.Wish.Add(item);
            _dbContext.SaveChanges();
        }
        public void AddComment(UserWish com)
        {
            var item = new Comment()
            {
                Comment1 = com.Notes,
                CommentTime = com.WishTime,
                UsersId = com.UsersId,
                UserComment = com.UserComment,
            };
            _dbContext.Comment.Add(item);
            _dbContext.SaveChanges();
        }

        public List<Wish> GetWish()
        {
            var items = new List<Wish>();
            items = _dbContext.Wish.Select(x => x).ToList();
            return items;
        }
    }
}

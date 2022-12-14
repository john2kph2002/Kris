using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KrisKringle.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KrisKringle.Services
{
    public partial interface IKrisKringleService
    {
        public List<Users> GetUsers();
        public List<Department> GetDepartment();
        public void AddUpdateDepartment(Department dept);
        public void AddUpdateUser(Users user);
        public List<SelectListItem> GetDepartmentDD();
        public List<WishDates> GetDates();
        public void DeleteWish(UserWish wish);
        public void AddWish(UserWish wish);
        public void UpdateWish(UserWish wish);
        public List<Wish> GetWish();
        public List<Comment> GetComments();
        public void AddComment(UserWish wish);
    }
}

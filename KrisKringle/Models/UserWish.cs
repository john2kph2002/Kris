using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KrisKringle.Models
{
    public class UserWish
    {
        public UserWish()
        {
            dates = new List<SelectListItem>();
            comments = new List<Comment>();
            wishes = new List<Wish>();
        }
        public int Id { get; set; }
        public string Wish1 { get; set; }
        public string Username { get; set; }
        public string Nick { get; set; }
        public DateTime? WishTime { get; set; }
        public DateTime? CommentTime { get; set; }
        public int UsersId { get; set; }
        public string Notes { get; set; }
        public string WishDate { get; set; }
        public string Dates { get; set; }
        public int UsrBtn { get; set; }

        public string UserComment{ get; set; }
        public string Email { get; set; }
        public string UsernameFoget { get; set; }
        public List<SelectListItem> dates { get; set; }
        public List<Comment> comments { get; set; }
        public List<Wish> wishes { get; set; }
    }
}

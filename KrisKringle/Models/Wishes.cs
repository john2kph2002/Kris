using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KrisKringle.Models
{
    public class Wishes
    {
        public int Id { get; set; }
        public string Wish1 { get; set; }
        public string Wish2 { get; set; }
        public string Wish3 { get; set; }
        public DateTime? WishTime { get; set; }
        public int? UsersId { get; set; }
        public string WishDate { get; set; }
        public string Username { get; set; }
    }
}

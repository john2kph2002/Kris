using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace KrisKringle.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string Comment1 { get; set; }
        public DateTime? CommentTime { get; set; }
        public DateTime? CommentDeletedTime { get; set; }
        public int? UsersId { get; set; }
        public string UserComment { get; set; }
    }
}

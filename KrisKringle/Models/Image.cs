using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace KrisKringle.Models
{
    public partial class Image
    {
        public int Id { get; set; }
        public string Image1 { get; set; }
        public string ImagePath { get; set; }
        public DateTime? ImageTime { get; set; }
        public DateTime? ImageDeleted { get; set; }
        public int? UsersId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace KrisKringle.Models
{
    public partial class Users
    {
        public int Id { get; set; }
        [MaxLength]
        public string Username { get; set; }
        [MaxLength]
        public string Password { get; set; }
        [MaxLength]
        public string Nick { get; set; }
        public bool? IsAdmin { get; set; }
        public int? DepartmentId { get; set; }
    }
}

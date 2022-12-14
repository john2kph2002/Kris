using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace KrisKringle.Models
{
    public partial class Usr
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UsernameFoget { get; set; }
        public string Nick { get; set; }
        public bool? IsAdmin { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        [Required]
        public string Email { get; set; }

    }
}

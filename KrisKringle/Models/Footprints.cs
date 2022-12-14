using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace KrisKringle.Models
{
    public partial class Footprints
    {
        public int Id { get; set; }
        public string Logs { get; set; }
        public DateTime? LogTime { get; set; }
    }
}

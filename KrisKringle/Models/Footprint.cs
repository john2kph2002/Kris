using System;
using System.Collections.Generic;

#nullable disable

namespace KrisKringle.Models
{
    public partial class Footprint
    {
        public int Id { get; set; }
        public string Logs { get; set; }
        public DateTime? LogTime { get; set; }
    }
}

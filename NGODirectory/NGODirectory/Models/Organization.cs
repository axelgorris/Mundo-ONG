using NGODirectory.Abstractions;
using System.Collections.Generic;

namespace NGODirectory.Models
{
    public class Organization : TableData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Web { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string LogoUrl { get; set; }
        public string Category { get; set; }

        //public virtual ICollection<Announcement> Announcements { get; set; }
    }
}

using NGODirectory.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGODirectory.Models
{
    public class Announcement : TableData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }        
    }
}

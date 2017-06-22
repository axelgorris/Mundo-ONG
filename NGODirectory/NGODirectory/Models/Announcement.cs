using NGODirectory.Abstractions;

namespace NGODirectory.Models
{
    public class Announcement : TableData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Organization { get; set; }
        public string Author { get; set; }
        public string ExternalUrl { get; set; }
    }
}

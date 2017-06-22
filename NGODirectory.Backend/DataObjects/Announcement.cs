using Microsoft.Azure.Mobile.Server;

namespace NGODirectory.Backend.DataObjects
{
    public class Announcement : EntityData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Organization { get; set; }
        public string Author { get; set; }
        public string ExternalUrl { get; set; }
    }
}
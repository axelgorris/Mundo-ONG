using Microsoft.Azure.Mobile.Server;

namespace NGODirectory.Backend.DataObjects
{
    public class Announcement : EntityData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
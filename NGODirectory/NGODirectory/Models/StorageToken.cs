using System;

namespace NGODirectory.Models
{
    public class StorageToken
    {
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public string SasToken { get; set; }
    }
}

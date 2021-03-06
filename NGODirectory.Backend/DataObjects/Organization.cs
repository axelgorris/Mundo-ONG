﻿using Microsoft.Azure.Mobile.Server;
using System.Collections.Generic;

namespace NGODirectory.Backend.DataObjects
{
    public class Organization : EntityData
    {
        public string AdminUser { get; set; }
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
    }
}
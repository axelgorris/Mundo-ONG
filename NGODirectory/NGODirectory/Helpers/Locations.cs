﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGODirectory.Helpers
{
    public static class Locations
    {
//#if DEBUG
        //public static readonly string AppServiceUrl = "http://localhost:50161/";
        //public static readonly string AlternateLoginHost = "https://ngodirectory.azurewebsites.net";
//#else
        public static readonly string AppServiceUrl = "https://ngodirectory.azurewebsites.net";
        public static readonly string AlternateLoginHost = null;
//#endif

        public static readonly string AadClientId = "a9c26aea-778e-450c-b2d0-f32ecd78cbfb";

        public static readonly string AadRedirectUri = "https://ngodirectory.azurewebsites.net/.auth/login/done";

        public static readonly string AadAuthority = "https://login.windows.net/axelgdoutlook.onmicrosoft.com";
    }
}
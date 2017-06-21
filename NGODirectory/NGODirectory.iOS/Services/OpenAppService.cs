using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using NGODirectory.Abstractions;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NGODirectory.iOS.Services
{
    public class OpenAppService : IOpenAppService
    {
        public Task<bool> Launch(string stringUri)
        {
            NSUrl request = new NSUrl(stringUri);

            UIApplication.SharedApplication.OpenUrl(request);

            return Task.FromResult(true);
        }
    }
}
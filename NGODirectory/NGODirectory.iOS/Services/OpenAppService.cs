using Foundation;
using NGODirectory.Abstractions;
using System.Threading.Tasks;
using UIKit;

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
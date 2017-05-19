using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace NGODirectory.Abstractions
{
    public interface ILoginProvider
    {
        Task LoginAsync(MobileServiceClient client);
    }
}

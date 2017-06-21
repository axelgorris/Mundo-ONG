using System.Threading.Tasks;

namespace NGODirectory.Abstractions
{
    public interface IOpenAppService
    {
        Task<bool> Launch(string stringUri);
    }
}

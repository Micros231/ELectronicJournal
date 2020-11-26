using ElectronicJournal.Application.AppService;
using System.Threading.Tasks;

namespace ElectronicJournal.Application.PrepareInitialize
{
    public interface IPrepareInitializeAppService : IAppService
    {
        Task<bool> IsInitialize();
        Task Initialize();
    }
}

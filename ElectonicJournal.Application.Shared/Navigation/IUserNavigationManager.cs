using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicJournal.Application.Navigation
{
    public interface IUserNavigationManager
    {
        Task<UserMenu> GetMenuAsync(string menuName, ClaimsPrincipal user);
        Task<IReadOnlyList<UserMenu>> GetMenusAsync(ClaimsPrincipal user);
    }
}

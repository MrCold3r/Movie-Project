using ASP_MVC_Movie.Models;
using ASP_MVC_Movie.ViewModel;

namespace ASP_MVC_Movie.Interfaces
{
    public interface IUserManagementService
    {
        // All method name says what it can

        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<bool> RemoveUser(string userId);
        Task<bool> EditUserInfo(string userId, EditUserVM model);

    }
}

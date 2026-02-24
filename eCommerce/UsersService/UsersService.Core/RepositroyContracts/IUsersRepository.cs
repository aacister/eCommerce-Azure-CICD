

using UserService.Core.Entities;

namespace UserService.Core.RepositroyContracts
{
    public interface IUsersRepository
    {
        Task<ApplicationUser?> AddUser(ApplicationUser user);
        Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password);
        Task<ApplicationUser?> GetUserByUserID(Guid? userID);
    }
}

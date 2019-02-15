namespace Shop.Web.Helpers
{
    using System.Threading.Tasks;
    using Data.Entities;
    using Microsoft.AspNetCore.Identity;

    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);
    }
}

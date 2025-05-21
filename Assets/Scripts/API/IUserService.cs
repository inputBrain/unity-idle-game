using System.Threading.Tasks;
using Model.User;

namespace API
{
    public interface IUserService
    {
        Task<UserModel> GetCurrentUserAsync();
    }
}

namespace EmployeeManagementLibrary.DB
{
    public interface IUserData
    {
        Task AddUser(UserModel user);
        Task<List<UserModel>> Users();
        Task<bool> CheckDetails(string email, string password);
        Task<UserModel> FindUser(string email, string answer);
        Task UpdateUser(UserModel userModel);
        Task<bool> IsAdministrator(string email);
        Task<UserModel> FindUserByEmail(string email);

    }
}
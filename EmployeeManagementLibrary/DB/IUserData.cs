
namespace EmployeeManagementLibrary.DB
{
    public interface IUserData
    {
        Task AddUser(UserModel user);
        Task<List<UserModel>> Users();
        Task<bool> CheckDetails(string email, string password);
    }
}
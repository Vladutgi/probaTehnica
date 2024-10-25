namespace EmployeeManagement.Services
{
    public interface ISessionService
    {
        event Action OnLoginStateChanged;

        Task ClearCurrentUser();
        Task ClearSavedEmail();
        Task<string> GetCurrentUser();
        Task<string> GetSavedEmail();
        Task<bool> GetUserRole();
        Task<bool> IsLoggedIn();
        Task Logout();
        Task SetCurrentUser(string username, bool role);
        Task SetSavedEmail(string email);
        void NavigateTo(string uri);
        string GetUri();
    }
}
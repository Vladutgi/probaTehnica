using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using System.Data;

namespace EmployeeManagement;
public class SessionService : ISessionService
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private readonly ProtectedLocalStorage _localStorage;
    private readonly NavigationManager _navigationManager;
    public event Action OnLoginStateChanged;

    public SessionService(ProtectedSessionStorage sessionStorage, ProtectedLocalStorage localStorage, NavigationManager navigationManager)
    {
        _sessionStorage = sessionStorage;
        _localStorage = localStorage;
        _navigationManager = navigationManager;
    }



    public async Task SetCurrentUser(string username, bool role)
    {
        await _sessionStorage.SetAsync("CurrentUser", username);
        await _sessionStorage.SetAsync("UserRole", role);

        OnLoginStateChanged?.Invoke();
    }


    public async Task<string> GetCurrentUser()
    {
        var result = await _sessionStorage.GetAsync<string>("CurrentUser");
        return result.Success ? result.Value : null;
    }
    public async Task<bool> GetUserRole()
    {
        var result = await _sessionStorage.GetAsync<bool>("UserRole");
        return result.Value;
    }

    public async Task ClearCurrentUser()
    {
        await _sessionStorage.DeleteAsync("CurrentUser");
        await _sessionStorage.DeleteAsync("UserRole");

    }

    public async Task Logout()
    {
        await ClearCurrentUser();
        OnLoginStateChanged?.Invoke();
        _navigationManager.NavigateTo("/Login");
    }
    public async Task<bool> IsLoggedIn()
    {
        return await GetCurrentUser() != null;
    }
    public async Task SetSavedEmail(string email)
    {
        await _localStorage.SetAsync("savedEmail", email);
    }
    public async Task<string> GetSavedEmail()
    {
        var result = await _localStorage.GetAsync<string>("savedEmail");
        return result.Success ? result.Value : null;
    }
    public async Task ClearSavedEmail()
    {
        await _localStorage.DeleteAsync("savedEmail");
    }

    public void NavigateTo(string uri)
    {
        _navigationManager.NavigateTo(uri);
    }

    public String GetUri()
    {
        return _navigationManager.Uri;
    }

}

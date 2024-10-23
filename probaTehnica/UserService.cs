namespace EmployeeManagement;

public class UserService
{
    private readonly IUserData _userData;
    private string _currentUser;
    private bool _userRole;

    public UserService(IUserData userData)
    {
        _userData = userData;
    }

    public async Task InitializeUserRole(string email)
    {
        _currentUser = email;
        var user = await _userData.FindUserByEmail(email);
        if (user != null)
        {
            _userRole = await _userData.IsAdministrator(email);

        }
    }

    public bool IsAdmin() { return _userRole; }
    public string GetCurrentUser() { return _currentUser; }
    
}

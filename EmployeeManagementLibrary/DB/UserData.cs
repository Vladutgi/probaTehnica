namespace EmployeeManagementLibrary.DB;
public class UserData : IUserData
{
    private readonly DBDataContext _context;
    public UserData(DBDataContext context)
    {
        _context = context;
    }

    public async Task<List<UserModel>> Users()
    {
        return await _context.Users.ToListAsync();
    }
    public async Task AddUser(UserModel user)
    {
        if (user != null)
        {
            var emailExists = await _context.Users.AnyAsync(d => d.Email.Equals(user.Email));
            if (emailExists == false)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            else { throw new InvalidOperationException("This email is already used"); }
        }

    }
    public async Task<bool> CheckDetails(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email);

        if (user.EncryptedPassword == password)
        {
            return true;

        }

        return false;


    }

}

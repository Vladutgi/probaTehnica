using System.Text;

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
    private async Task<bool> AnyAdministrators()
    {
        return await _context.Users.AnyAsync(u => u.Role == "Administrator");
    }
    public async Task AddUser(UserModel user)
    {
        if (user != null)
        {
            var emailExists = await _context.Users.AnyAsync(d => d.Email.Equals(user.Email));
            if (emailExists == false)
            {
                if ( await AnyAdministrators() == false)
                {
                    user.Role = "Administrator";
                }
                else
                {
                    user.Role = "";
                }
                user.EncryptedPassword = PasswordHash(user.EncryptedPassword);
                user.EncryptedQuestionAnswer = PasswordHash(user.EncryptedQuestionAnswer);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            else { throw new InvalidOperationException("This email is already used"); }
        }

    }
    public async Task<bool> CheckDetails(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email);

        if (user!= null &&user.EncryptedPassword == PasswordHash(password))
        {
            return true;

        }

        return false;


    }
    public async Task<UserModel> FindUser(string email,string answer)
    {
        var result = await _context.Users.FirstOrDefaultAsync(u=> u.Email == email);
        if (result != null)
        {
            if (result.EncryptedQuestionAnswer == PasswordHash(answer))
            {
                return result;
            }
        }
        return null;
    }
    public async Task UpdateUser(UserModel userModel)
    {
        if (userModel != null)
        {
            var existingUser = await _context.Users.FindAsync(userModel.UserId);
            if (existingUser != null)
            {
                existingUser.EncryptedPassword = PasswordHash(userModel.EncryptedPassword);
                existingUser.LastModified = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            else { throw new InvalidOperationException("User does not exist"); }
        }
    }
    public string PasswordHash(string Pasword)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            byte[] hashedPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(Pasword));
            return Convert.ToBase64String(hashedPassword);
        }

    }

    public async Task<bool> IsAdministrator(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email);

        if (user != null && user.Role == "Administrator")
        {
            return true;

        }

        return false;


    }
    public async Task<UserModel> FindUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }
}

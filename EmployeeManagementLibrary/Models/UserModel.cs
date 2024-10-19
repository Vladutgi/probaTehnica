namespace EmployeeManagementLibrary.Models;

[Table("Users")]
public class UserModel
{
    [Key]//PrimaryKey
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//auto generated value
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string EncryptedPassword { get; set; }
    public string SecretQuestion { get; set; }
    public string EncryptedQuestionAnswer { get; set; }
    public string Role { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }


}

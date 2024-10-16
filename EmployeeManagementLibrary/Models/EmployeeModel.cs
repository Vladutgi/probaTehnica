using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EmployeeManagementLibrary.Models;

[Table("Employees")]
public class EmployeeModel
{
    [Key]//PrimaryKey
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//auto generated value
    public int EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Department { get; set; }
    public int Salary { get; set; }


}


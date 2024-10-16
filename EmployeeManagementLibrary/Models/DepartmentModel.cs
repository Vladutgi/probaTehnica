namespace EmployeeManagementLibrary.Models;

[Table("Departments")]
public class DepartmentModel
{
    [Key]//PrimaryKey
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//auto generated value
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }


}


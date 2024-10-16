
namespace EmployeeManagementLibrary.DB
{
    public interface IEmployeeData
    {
        Task AddEmployee(EmployeeModel employee);
        Task<List<EmployeeModel>> Employees();
    }
}
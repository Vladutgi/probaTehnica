
namespace EmployeeManagementLibrary.DB
{
    public interface IEmployeeData
    {
        Task AddEmployee(EmployeeModel employee);
        Task<List<EmployeeModel>> Employees();
        Task RemoveEmployee(int employeeId);
        Task<EmployeeModel> FindEmployee(int employeeId);
        Task UpdateEmployee(EmployeeModel employee);
    }
}
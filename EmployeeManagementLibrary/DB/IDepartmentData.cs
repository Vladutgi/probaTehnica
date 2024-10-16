
namespace EmployeeManagementLibrary.DB
{
    public interface IDepartmentData
    {
        Task<List<DepartmentModel>> Departments();
    }
}
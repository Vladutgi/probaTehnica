
namespace EmployeeManagementLibrary.DB
{
    public interface IDepartmentData
    {
        Task<List<DepartmentModel>> Departments();

        Task AddDepartment(DepartmentModel departmentModel);
        Task RemoveDepartment(int departmentId);
        Task<DepartmentModel> FindDepartment(int departmentId);
        Task UpdateDepartment(DepartmentModel departmentModel);
    }
}
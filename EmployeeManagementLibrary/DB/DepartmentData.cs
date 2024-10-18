namespace EmployeeManagementLibrary.DB
{
    public class DepartmentData : IDepartmentData
    {
        private readonly DBDataContext _context;
        public DepartmentData(DBDataContext context)
        {
            _context = context;
        }

        public async Task<List<DepartmentModel>> Departments()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task AddDepartment(DepartmentModel departmentModel)
        {
            if (departmentModel != null)
            {
                _context.Departments.Add(departmentModel);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveDepartment(int departmentId)
        {
            var department = await _context.Departments.FindAsync(departmentId);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<DepartmentModel> FindDepartment(int departmentId)
        {
            return await _context.Departments.FindAsync(departmentId);
        }
        public async Task UpdateDepartment(DepartmentModel departmentModel)
        {
            if (departmentModel != null)
            {
                var existingDepartment = await _context.Departments.FindAsync(departmentModel.DepartmentId);
                if (existingDepartment != null)
                {
                    existingDepartment.DepartmentName = departmentModel.DepartmentName;
                }
                else { throw new InvalidOperationException("Department does not exist"); }
            }
        }
    }
}
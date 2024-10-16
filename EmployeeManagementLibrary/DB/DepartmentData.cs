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
    }
}
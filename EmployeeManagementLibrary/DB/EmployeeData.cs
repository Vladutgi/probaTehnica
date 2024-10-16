namespace EmployeeManagementLibrary.DB;

public class EmployeeData : IEmployeeData
{
    private readonly DBDataContext _context;
    public EmployeeData(DBDataContext context)
    {
        _context = context;
    }

    public async Task<List<EmployeeModel>> Employees()
    {
        return await _context.Employees.ToListAsync();
    }
    public async Task AddEmployee(EmployeeModel employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
    }
}



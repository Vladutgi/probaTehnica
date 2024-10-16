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
        if (employee != null)
        {
            var departmentExists = await _context.Departments.AnyAsync(d => d.DepartmentName.Equals(employee.Department));
            if (departmentExists == true)
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
            }
            else { throw new InvalidOperationException("Department does not exist"); }
        }

    }

    public async Task RemoveEmployee(int employeeId)
    {
        var employee = await _context.Employees.FindAsync(employeeId);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<EmployeeModel> FindEmployee(int employeeId)
    {
        return await _context.Employees.FindAsync(employeeId);
    }

    public async Task UpdateEmployee(EmployeeModel employee)
    {
        
        if (employee != null)
        {
            var existingEmployee = await _context.Employees.FindAsync(employee.EmployeeId);
            if(existingEmployee != null)
            {
                existingEmployee.FirstName = employee.FirstName;
                existingEmployee.LastName = employee.LastName;
                existingEmployee.Department = employee.Department;
                existingEmployee.Salary = employee.Salary;
                await _context.SaveChangesAsync();
            }
            else { throw new InvalidOperationException("Employee does not exist"); }
        }
    }
}



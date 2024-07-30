using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;

namespace EmployeeManagement.Repositories.Implements;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly DapperContext _context;

    public EmployeeRepository(DapperContext context)
    {
        _context = context;
    }
    
    public Task Create()
    {
        throw new NotImplementedException();
    }

    public Task Remove()
    {
        throw new NotImplementedException();
    }

    public Task Update()
    {
        throw new NotImplementedException();
    }

    public Task<Employee> GetEmployeeDetailByEmployeeId(Guid employeeId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Employee>> GetListEmployees()
    {
        throw new NotImplementedException();
    }

    public Task<List<Employee>> SearchEmployee()
    {
        throw new NotImplementedException();
    }
}
using EmployeeManagement.Models;

namespace EmployeeManagement.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task Create();

    Task Remove();

    Task Update();

    Task<Employee> GetEmployeeDetailByEmployeeId(Guid employeeId);

    Task<List<Employee>> GetListEmployees();

    Task<List<Employee>> SearchEmployee();
}
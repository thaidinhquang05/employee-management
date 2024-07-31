using EmployeeManagement.DTOs;
using EmployeeManagement.Models;

namespace EmployeeManagement.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task Create(CreateEmployeeRequest request);

    Task Remove(Guid employeeId);

    Task Update();

    Task<Employee> GetEmployeeDetailByEmployeeId(Guid employeeId);

    Task<List<Employee>> GetListEmployees();

    Task<List<Employee>> SearchEmployee();
}
using EmployeeManagement.DTOs;
using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class EmployeeController : Controller
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpPost]
    public async Task<ApiResponse> Create(CreateEmployeeRequest request)
    {
        try
        {
            await _employeeRepository.Create(request);
            return new ApiResponse
            {
                Code = 200,
                Message = "Created Successfully!"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse
            {
                Code = 500,
                Message = ex.Message
            };
        }
    }

    [HttpDelete]
    public async Task<ApiResponse> Delete(Guid employeeId)
    {
        if (employeeId == Guid.Empty)
            return new ApiResponse
            {
                Code = 500,
                Message = "Employee Id cannot be null!"
            };

        try
        {
            await _employeeRepository.Remove(employeeId);
            return new ApiResponse
            {
                Code = 200,
                Message = "Deleted Successfully!"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse
            {
                Code = 500,
                Message = ex.Message
            };
        }
    }

    [HttpPut]
    public async Task<ApiResponse> Update([FromQuery] Guid employeeId, [FromBody] UpdateEmployeeRequest request)
    {
        if (employeeId == Guid.Empty)
            return new ApiResponse
            {
                Code = 500,
                Message = "Employee Id cannot be null!"
            };
        try
        {
            await _employeeRepository.Update(employeeId, request);
            return new ApiResponse
            {
                Code = 200,
                Message = "Updated Successfully!"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse
            {
                Code = 500,
                Message = ex.Message
            };
        }
    }

    [HttpGet]
    public async Task<ApiResponse> GetEmployeeDetailById(Guid employeeId)
    {
        if (employeeId == Guid.Empty)
            return new ApiResponse
            {
                Code = 500,
                Message = "Employee Id cannot be null!"
            };
        try
        {
            var response = await _employeeRepository.GetEmployeeDetailByEmployeeId(employeeId);
            if (response == null)
                return new ApiResponse
                {
                    Code = 404,
                    Message = "Object not found!"
                };
            return new ApiResponse
            {
                Code = 200,
                Message = "Get Data Successfully!",
                Data = response
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse
            {
                Code = 500,
                Message = ex.Message
            };
        }
    }

    [HttpPost]
    public async Task<ApiResponse> SearchEmployee(SearchEmployeesRequest request)
    {
        try
        {
            var response = await _employeeRepository.SearchEmployee(request);
            if (response.Count == 0)
                return new ApiResponse
                {
                    Code = 404,
                    Message = "Object not found!"
                };
            return new ApiResponse
            {
                Code = 200,
                Message = "Get Data Successfully!",
                Data = response
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse
            {
                Code = 500,
                Message = ex.Message
            };
        }
    }
}
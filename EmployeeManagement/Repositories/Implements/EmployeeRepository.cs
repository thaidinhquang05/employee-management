using System.Data;
using System.Text.RegularExpressions;
using Dapper;
using EmployeeManagement.DTOs;
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

    public async Task Create(CreateEmployeeRequest request)
    {
        var query = @"INSERT INTO employee 
                       (EmployeeId,
                        FullName,
                        Gender,
                        Dob,
                        Email,
                        Address,
                        SearchText,
                        IdentifierId,
                        ExpireDate,
                        IssuedBy,
                        MobilePhone,
                        LandlinePhone,
                        BankAccount,
                        BankName,
                        BranchBank,
                        Position,
                        Department)
                    VALUES (@employeeId, 
                            @fullName, 
                            @gender, 
                            @dob, 
                            @email, 
                            @address, 
                            @searchText,
                            @identifierId,
                            @expireDate,
                            @issuedBy,
                            @mobilePhone,
                            @landlinePhone,
                            @bankAccount,
                            @bankName,
                            @branchBank,
                            @position,
                            @department)";

        var id = 0;
        var latestEmployee = await GetLatestEmployee();
        if (latestEmployee != null)
        {
            id = latestEmployee.Id;
        }

        var parameters = new DynamicParameters();
        parameters.Add("employeeId", Guid.NewGuid(), DbType.Guid);
        parameters.Add("fullName", request.FullName, DbType.String);
        parameters.Add("gender", request.Gender, DbType.Int32);
        parameters.Add("dob", request.Dob, DbType.DateTime);
        if (!string.IsNullOrEmpty(request.Email) && IsValidEmail(request.Email))
        {
            parameters.Add("email", request.Email, DbType.String);
        }

        parameters.Add("address", request.Address, DbType.String);
        parameters.Add("searchText", $"{id} - {request.FullName}", DbType.String);
        parameters.Add("identifierId", request.IdentifierId, DbType.String);
        parameters.Add("expireDate", request.ExpireDate, DbType.DateTime);
        parameters.Add("issuedBy", request.IssuedBy, DbType.Int32);
        if (!string.IsNullOrEmpty(request.MobilePhone) && IsValidPhoneNumber(request.MobilePhone))
        {
            parameters.Add("mobilePhone", request.MobilePhone, DbType.String);
        }

        if (!string.IsNullOrEmpty(request.LandlinePhone) && IsValidPhoneNumber(request.LandlinePhone))
        {
            parameters.Add("landlinePhone", request.LandlinePhone, DbType.String);
        }

        parameters.Add("bankAccount", request.BankAccount, DbType.String);
        parameters.Add("bankName", request.BankName, DbType.String);
        parameters.Add("branchBank", request.BranchBank, DbType.String);
        parameters.Add("position", request.Position, DbType.Int32);
        parameters.Add("department", request.Department, DbType.Int32);
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, parameters);
    }

    public async Task Remove(Guid employeeId)
    {
        var query = "DELETE FROM employee WHERE EmployeeId = @employeeId";
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, new { employeeId });
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

    private async Task<Employee?> GetLatestEmployee()
    {
        var query = "SELECT * FROM employee ORDER BY Id DESC";
        using var connection = _context.CreateConnection();
        IEnumerable<Employee?> employees = await connection.QueryAsync<Employee>(query);
        return employees.FirstOrDefault();
    }

    private bool IsValidEmail(string email)
    {
        var pattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }

    private bool IsValidPhoneNumber(string phoneNumber)
    {
        // Regex pattern for validating phone numbers
        var pattern = @"^\+?\d{1,4}?[-.\s]?(\d{1,3}?[-.\s]?){1,5}(\d{1,4})$";
        return Regex.IsMatch(phoneNumber, pattern);
    }
}
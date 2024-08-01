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
        
        if (!string.IsNullOrEmpty(request.Email) && !IsValidEmail(request.Email))
        {
            throw new Exception("Email is Invalid!");
        }
        if (!string.IsNullOrEmpty(request.MobilePhone) && !IsValidPhoneNumber(request.MobilePhone))
        {
            throw new Exception("MobilePhone is invalid!");
        }
        if (!string.IsNullOrEmpty(request.LandlinePhone) && !IsValidPhoneNumber(request.LandlinePhone))
        {
            throw new Exception("LandlinePhone is invalid!");
        }

        var parameters = new DynamicParameters();
        parameters.Add("employeeId", Guid.NewGuid(), DbType.Guid);
        parameters.Add("fullName", request.FullName, DbType.String);
        parameters.Add("gender", request.Gender, DbType.Int32);
        parameters.Add("dob", request.Dob, DbType.DateTime);
        parameters.Add("email", request.Email, DbType.String);
        parameters.Add("address", request.Address, DbType.String);
        parameters.Add("searchText", $"{id + 1} - {request.FullName}", DbType.String);
        parameters.Add("identifierId", request.IdentifierId, DbType.String);
        parameters.Add("expireDate", request.ExpireDate, DbType.DateTime);
        parameters.Add("issuedBy", request.IssuedBy, DbType.Int32);
        parameters.Add("mobilePhone", request.MobilePhone, DbType.String);
        parameters.Add("landlinePhone", request.LandlinePhone, DbType.String);
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

    public async Task Update(Guid employeeId, UpdateEmployeeRequest request)
    {
        var query = @"UPDATE employee SET 
                        FullName = @fullName,
                        Gender = @gender,
                        Dob = @dob,
                        Email = @email,
                        Address = @address,
                        IdentifierId = @identifierId,
                        ExpireDate = @expireDate,
                        IssuedBy = @issuedBy,
                        MobilePhone = @mobilePhone,
                        LandlinePhone = @landlinePhone,
                        BankAccount = @bankAccount,
                        BankName = @bankName,
                        BranchBank = @branchBank,
                        Position = @position,
                        Department = @department
                    WHERE EmployeeId = @employeeId";
        
        if (!string.IsNullOrEmpty(request.Email) && !IsValidEmail(request.Email))
        {
            throw new Exception("Email is Invalid!");
        }
        if (!string.IsNullOrEmpty(request.MobilePhone) && !IsValidPhoneNumber(request.MobilePhone))
        {
            throw new Exception("MobilePhone is invalid!");
        }
        if (!string.IsNullOrEmpty(request.LandlinePhone) && !IsValidPhoneNumber(request.LandlinePhone))
        {
            throw new Exception("LandlinePhone is invalid!");
        }
        
        var parameters = new DynamicParameters();
        parameters.Add("employeeId", employeeId, DbType.Guid);
        parameters.Add("fullName", request.FullName, DbType.String);
        parameters.Add("gender", request.Gender, DbType.Int32);
        parameters.Add("dob", request.Dob, DbType.DateTime);
        parameters.Add("email", request.Email, DbType.String);
        parameters.Add("address", request.Address, DbType.String);
        parameters.Add("identifierId", request.IdentifierId, DbType.String);
        parameters.Add("expireDate", request.ExpireDate, DbType.DateTime);
        parameters.Add("issuedBy", request.IssuedBy, DbType.Int32);
        parameters.Add("mobilePhone", request.MobilePhone, DbType.String);
        parameters.Add("landlinePhone", request.LandlinePhone, DbType.String);
        parameters.Add("bankAccount", request.BankAccount, DbType.String);
        parameters.Add("bankName", request.BankName, DbType.String);
        parameters.Add("branchBank", request.BranchBank, DbType.String);
        parameters.Add("position", request.Position, DbType.Int32);
        parameters.Add("department", request.Department, DbType.Int32);

        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(query, parameters);
    }

    public async Task<Employee?> GetEmployeeDetailByEmployeeId(Guid employeeId)
    {
        var query = "SELECT * FROM employee WHERE EmployeeId = @employeeId";
        using var connection = _context.CreateConnection();
        var employee = await connection.QuerySingleOrDefaultAsync<Employee>(query, new {employeeId});
        return employee;
    }

    public async Task<List<Employee>> SearchEmployee(SearchEmployeesRequest request)
    {
        var query = "SELECT * FROM employee WHERE true";
        if (!string.IsNullOrEmpty(request.SearchText))
        {
            query += " AND SearchText LIKE @searchText";
        }
        var parameters = new DynamicParameters();
        parameters.Add("searchText", $"%{request.SearchText}%", DbType.String);
        
        using var connection = _context.CreateConnection();
        var employees = await connection.QueryAsync<Employee>(query, parameters);
        employees = employees.Skip(request.Offset).Take(request.PageSize);
        return employees.ToList();
    }

    private async Task<Employee?> GetLatestEmployee()
    {
        var query = "SELECT * FROM employee ORDER BY Id DESC";
        using var connection = _context.CreateConnection();
        var employee = await connection.QuerySingleOrDefaultAsync<Employee>(query);
        return employee;
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
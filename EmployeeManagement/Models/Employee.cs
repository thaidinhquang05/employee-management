using EmployeeManagement.Constants;

namespace EmployeeManagement.Models;

public class Employee
{
    public int Id { get; set; }

    public Guid EmployeeId { get; set; }

    public required string FullName { get; set; }

    public GenderEnum Gender { get; set; }

    public DateTime Dob { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? SearchText { get; set; }

    public string? IdentifierId { get; set; }

    public DateTime DataRange { get; set; }

    public IssuedByEnum IssuedBy { get; set; }

    public string? MobilePhone { get; set; }

    public string? LandlinePhone { get; set; }

    public string? BankAccount { get; set; }

    public string? BankName { get; set; }

    public string? BranchBank { get; set; }

    public PositionEnum Position { get; set; }

    public DepartmentEnum Department { get; set; }
}
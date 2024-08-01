using System.ComponentModel.DataAnnotations;
using EmployeeManagement.Constants;

namespace EmployeeManagement.DTOs;

public class UpdateEmployeeRequest
{
    [Required(ErrorMessage = "Full Name is required!")]
    public required string FullName { get; set; }

    public GenderEnum? Gender { get; set; } = GenderEnum.Male;

    public DateTime? Dob { get; set; } = DateTime.Now;

    public string? Email { get; set; } = "abc@gmail.com";

    public string? Address { get; set; } = "temp address";

    public string? IdentifierId { get; set; } = "0123456789";

    public DateTime ExpireDate { get; set; } = DateTime.Now;

    public IssuedByEnum? IssuedBy { get; set; } = IssuedByEnum.Issuer1;

    public string? MobilePhone { get; set; } = "0987654321";

    public string? LandlinePhone { get; set; } = "0987654321";

    public string? BankAccount { get; set; } = "0987654321";

    public string? BankName { get; set; } = "Techcombank";

    public string? BranchBank { get; set; } = "Hanoi";

    public PositionEnum Position { get; set; } = PositionEnum.InternDeveloper;

    public DepartmentEnum Department { get; set; } = DepartmentEnum.Dep1;
}
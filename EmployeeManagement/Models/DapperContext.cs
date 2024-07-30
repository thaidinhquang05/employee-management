using System.Data;
using MySqlConnector;

namespace EmployeeManagement.Models;

public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string? _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = configuration.GetConnectionString("ConStr");
    }

    public IDbConnection CreateConnection() => new MySqlConnection(_connectionString);
}
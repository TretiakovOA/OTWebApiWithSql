using Microsoft.Data.SqlClient;
using OTWebApiWithSql.Interfaces.Repositories;
using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Repositories;

public class OperatorRepository : IOperatorRepository
{
    private readonly string _connectionString;

    public OperatorRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SqlServer")
            ?? throw new InvalidOperationException("Connection string was not found");
    }

    public IEnumerable<Operator> GetAll(int page, int size, out int totalCount)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        totalCount = GetTotalCount(connection);

        var operators = new List<Operator>();
        var query = "SELECT Id, FullName, Email, Phone FROM Operators " +
            "ORDER BY Id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Offset", (page - 1) * size);
        command.Parameters.AddWithValue("@PageSize", size);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            operators.Add(new Operator
            {
                Id = (int)reader["Id"],
                FullName = (string)reader["FullName"],
                Email = (string)reader["Email"],
                Phone = (string)reader["Phone"]
            });
        }
        return operators;
    }

    private static int GetTotalCount(SqlConnection connection)
    {
        using var command = new SqlCommand("SELECT COUNT(*) FROM Clients", connection);
        return Convert.ToInt32(command.ExecuteScalar());
    }

    public Operator? GetById(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT Id, FullName, Email, Phone FROM Operators WHERE Id = @Id";
        using var cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@Id", id);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Operator
            {
                Id = (int)reader["Id"],
                FullName = (string)reader["FullName"],
                Email = (string)reader["Email"],
                Phone = (string)reader["Phone"]
            };
        }
        return null;
    }

    public int Add(Operator entity)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = @"INSERT INTO Operators (FullName, Email, Phone)
                    OUTPUT INSERTED.Id
                    VALUES (@FullName, @Email, @Phone)";

        using var cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@FullName", entity.FullName);
        cmd.Parameters.AddWithValue("@Email", entity.Email);
        cmd.Parameters.AddWithValue("@Phone", entity.Phone);

        return (int)cmd.ExecuteScalar();
    }

    public void Update(Operator entity)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = @"UPDATE Operators SET
                    FullName = @FullName,
                    Email = @Email,
                    Phone = @Phone
                    WHERE Id = @Id";

        using var cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@Id", entity.Id);
        cmd.Parameters.AddWithValue("@FullName", entity.FullName);
        cmd.Parameters.AddWithValue("@Email", entity.Email);
        cmd.Parameters.AddWithValue("@Phone", entity.Phone);

        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = "DELETE FROM Operators WHERE Id = @Id";
        using var cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@Id", id);

        cmd.ExecuteNonQuery();
    }

}

using Microsoft.Data.SqlClient;
using OTWebApiWithSql.Interfaces.Repositories;
using OTWebApiWithSql.Models;

using System.Text.RegularExpressions;

namespace OTWebApiWithSql.Repositories;

public class CreditApplicationRepository : ICreditApplicationRepository
{
    private readonly string _connectionString;

    public CreditApplicationRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SqlServer")
            ?? throw new InvalidOperationException("Connection string was not found");
    }

    public CreditApplication? GetById(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT * FROM CreditApplications WHERE Id = @Id";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new CreditApplication
            {
                Id = (int)reader["Id"],
                LoanPurpose = (string)reader["LoanPurpose"],
                LoanAmount = (decimal)reader["LoanAmount"],
                ClientIncome = (decimal)reader["ClientIncome"],
                CreditProductId = (int)reader["CreditProductId"]
            };
        }
        return null;
    }

    public IEnumerable<CreditApplication> GetAll(int pageNumber, int pageSize, out int totalCount)
    {
        var applications = new List<CreditApplication>();
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        totalCount = GetTotalCount(connection);

        var query = "SELECT * FROM CreditApplications " +
            "ORDER BY Id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
        command.Parameters.AddWithValue("@PageSize", pageSize);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            applications.Add(new CreditApplication
            {
                Id = (int)reader["Id"],
                LoanPurpose = (string)reader["LoanPurpose"],
                LoanAmount = (decimal)reader["LoanAmount"],
                ClientIncome = (decimal)reader["ClientIncome"],
                CreditProductId = (int)reader["CreditProductId"]
            });
        }
        return applications;
    }

    private static int GetTotalCount(SqlConnection connection)
    {
        using var command = new SqlCommand("SELECT COUNT(*) FROM CreditApplications", connection);
        return Convert.ToInt32(command.ExecuteScalar());
    }

    public int Add(CreditApplication application)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = @"INSERT INTO CreditApplications 
                    (LoanPurpose, LoanAmount, ClientIncome, CreditProductId)
                    OUTPUT INSERTED.Id
                    VALUES (@LoanPurpose, @LoanAmount, @ClientIncome, @CreditProductId)";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@LoanPurpose", application.LoanPurpose);
        command.Parameters.AddWithValue("@LoanAmount", application.LoanAmount);
        command.Parameters.AddWithValue("@ClientIncome", application.ClientIncome);
        command.Parameters.AddWithValue("@CreditProductId", application.CreditProductId);

        return (int)command.ExecuteScalar();
    }

    public void Update(CreditApplication application)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = @"UPDATE CreditApplications SET 
                    LoanPurpose = @LoanPurpose,
                    LoanAmount = @LoanAmount,
                    ClientIncome = @ClientIncome,
                    CreditProductId = @CreditProductId
                    WHERE Id = @Id";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", application.Id);
        command.Parameters.AddWithValue("@LoanPurpose", application.LoanPurpose);
        command.Parameters.AddWithValue("@LoanAmount", application.LoanAmount);
        command.Parameters.AddWithValue("@ClientIncome", application.ClientIncome);
        command.Parameters.AddWithValue("@CreditProductId", application.CreditProductId);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = "DELETE FROM CreditApplications WHERE Id = @Id";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        command.ExecuteNonQuery();
    }
}

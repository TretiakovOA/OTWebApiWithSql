using Microsoft.Data.SqlClient;
using OTWebApiWithSql.Interfaces.Repositories;
using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Repositories;

public class CreditProductRepository: ICreditProductRepository
{
    private readonly string _connectionString;

    public CreditProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SqlServer")
            ?? throw new InvalidOperationException("Connection string was not found");
    }

    public CreditProduct? GetById(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT * FROM CreditProducts WHERE Id = @Id";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new CreditProduct
            {
                Id = (int)reader["Id"],
                ProductName = (string)reader["ProductName"],
                InterestRate = (decimal)reader["InterestRate"]
            };
        }
        return null;
    }

    public IEnumerable<CreditProduct> GetAll(int pageNumber, int pageSize, out int totalCount)
    {
        var products = new List<CreditProduct>();
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        totalCount = GetTotalCount(connection);

        var query = "SELECT * FROM CreditProducts " +
            "ORDER BY Id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
        command.Parameters.AddWithValue("@PageSize", pageSize);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            products.Add(new CreditProduct
            {
                Id = (int)reader["Id"],
                ProductName = (string)reader["ProductName"],
                InterestRate = (decimal)reader["InterestRate"]
            });
        }
        return products;
    }

    private static int GetTotalCount(SqlConnection connection)
    {
        using var command = new SqlCommand("SELECT COUNT(*) FROM CreditProducts", connection);
        return Convert.ToInt32(command.ExecuteScalar());
    }

    public int Add(CreditProduct product)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = @"INSERT INTO CreditProducts 
                    (ProductName, InterestRate)
                    OUTPUT INSERTED.Id
                    VALUES (@ProductName, @InterestRate)";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@ProductName", product.ProductName);
        command.Parameters.AddWithValue("@InterestRate", product.InterestRate);

        return (int)command.ExecuteScalar();
    }

    public void Update(CreditProduct product)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = @"UPDATE CreditProducts SET 
                    ProductName = @ProductName,
                    InterestRate = @InterestRate
                    WHERE Id = @Id";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", product.Id);
        command.Parameters.AddWithValue("@ProductName", product.ProductName);
        command.Parameters.AddWithValue("@InterestRate", product.InterestRate);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = "DELETE FROM CreditProducts WHERE Id = @Id";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        command.ExecuteNonQuery();
    }
}

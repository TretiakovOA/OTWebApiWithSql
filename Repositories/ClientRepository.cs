using Microsoft.Data.SqlClient;
using OTWebApiWithSql.Interfaces.Repositories;
using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly string _connectionString;

    public ClientRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SqlServer")
            ?? throw new InvalidOperationException("Connection string was not found");
    }

    public int Add(Client client)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = "INSERT INTO Clients (FullName, Age, Workplace, Phone) OUTPUT INSERTED.Id VALUES (@FullName, @Age, @Workplace, @Phone)";
        using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@FullName", client.FullName);
        command.Parameters.AddWithValue("@Age", client.Age);
        command.Parameters.AddWithValue("@Workplace", client.Workplace);
        command.Parameters.AddWithValue("@Phone", client.Phone);

        return (int)command.ExecuteScalar();
    }

    public Client? GetById(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT Id, FullName, Age, Workplace, Phone FROM Clients WHERE Id = @Id";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Client
            {
                Id = (int)reader["Id"],
                FullName = (string)reader["FullName"],
                Age = (int)reader["Age"],
                Workplace = reader["Workplace"] as string,
                Phone = (string)reader["Phone"]
            };
        }
        return null;
    }

    public IEnumerable<Client> GetAll(int pageNumber, int pageSize, out int totalCount)
    {        
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        totalCount = GetTotalCount(connection);

        var clients = new List<Client>();
        var query = "SELECT Id, FullName, Age, Workplace, Phone FROM Clients " +
            "ORDER BY Id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
        command.Parameters.AddWithValue("@PageSize", pageSize);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            clients.Add(new Client
            {
                Id = (int)reader["Id"],
                FullName = (string)reader["FullName"],
                Age = (int)reader["Age"],
                Workplace = reader["Workplace"] as string,
                Phone = (string)reader["Phone"]
            });
        }
        return clients;
    }

    private static int GetTotalCount(SqlConnection connection)
    {
        using var command = new SqlCommand("SELECT COUNT(*) FROM Clients", connection);
        return Convert.ToInt32(command.ExecuteScalar());
    }

    public void Update(Client client)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = @"UPDATE Clients SET 
                    FullName = @FullName,
                    Age = @Age,
                    Workplace = @Workplace,
                    Phone = @Phone
                    WHERE Id = @Id";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", client.Id);
        command.Parameters.AddWithValue("@FullName", client.FullName);
        command.Parameters.AddWithValue("@Age", client.Age);
        command.Parameters.AddWithValue("@Workplace", client.Workplace);
        command.Parameters.AddWithValue("@Phone", client.Phone);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = "DELETE FROM Clients WHERE Id = @Id";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        command.ExecuteNonQuery();
    }
}

using Microsoft.Data.SqlClient;
using OTWebApiWithSql.Interfaces.Repositories;
using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Repositories;

public class CallRepository: ICallRepository
{
    private readonly string _connectionString;

    public CallRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SqlServer")
            ?? throw new InvalidOperationException("Connection string was not found");
    }

    public Call? GetById(int id)
    {
        Call? call = null;
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            var query = "SELECT Id, ScheduledDate, Result, IsProcessed FROM Calls WHERE Id = @Id";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                call = new Call
                {
                    Id = (int)reader["Id"],
                    ScheduledDate = (DateTime)reader["ScheduledDate"],
                    Result = reader["Result"] as string ?? string.Empty,
                    IsProcessed = (bool)reader["IsProcessed"]
                };
            }
        }
        if (call != null) 
            call.CallAssignments = GetAssignments(id);

        return call;
    }

    public IEnumerable<Call> GetAll(int pageNumber, int pageSize, out int totalCount)
    {
        var calls = new List<Call>();
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            totalCount = GetTotalCount(connection);

            var query = "SELECT Id, ScheduledDate, Result, IsProcessed FROM Calls " +
                "ORDER BY Id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                calls.Add(new Call
                {
                    Id = (int)reader["Id"],
                    ScheduledDate = (DateTime)reader["ScheduledDate"],
                    Result = reader["Result"] as string ?? string.Empty,
                    IsProcessed = (bool)reader["IsProcessed"]
                });
            }
        }
        foreach (var call in calls)
        {
            call.CallAssignments = GetAssignments(call.Id);
        }
        return calls;
    }

    private static int GetTotalCount(SqlConnection connection)
    {
        using var command = new SqlCommand("SELECT COUNT(*) FROM Calls", connection);
        return Convert.ToInt32(command.ExecuteScalar());
    }

    private List<CallAssignment> GetAssignments(int callId)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var assignments = new List<CallAssignment>();
        var query = "SELECT CallId, OperatorId, AssignmentDate FROM CallAssignments WHERE CallId = @CallId";
        var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@CallId", callId);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            assignments.Add(new CallAssignment
            {
                CallId = (int)reader["CallId"],
                OperatorId = (int)reader["OperatorId"],
                AssignmentDate = (DateTime)reader["AssignmentDate"]
            });
        }

        return assignments;
    }

    public int Add(Call call)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = @"INSERT INTO Calls 
                    (ScheduledDate, Result, IsProcessed)
                    OUTPUT INSERTED.Id
                    VALUES (@ScheduledDate, @Result, @IsProcessed)";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@ScheduledDate", call.ScheduledDate);
        command.Parameters.AddWithValue("@Result", call.Result);
        command.Parameters.AddWithValue("@IsProcessed", call.IsProcessed);

        return (int)command.ExecuteScalar();
    }

    public void Update(Call call)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = @"UPDATE Calls SET 
                    ScheduledDate = @ScheduledDate,
                    Result = @Result,
                    IsProcessed = @IsProcessed
                    WHERE Id = @Id";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", call.Id);
        command.Parameters.AddWithValue("@ScheduledDate", call.ScheduledDate);
        command.Parameters.AddWithValue("@Result", call.Result);
        command.Parameters.AddWithValue("@IsProcessed", call.IsProcessed);

        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = "DELETE FROM Calls WHERE Id = @Id";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        command.ExecuteNonQuery();
    }
}

using Microsoft.Data.SqlClient;
using OTWebApiWithSql.Interfaces.Repositories;

namespace OTWebApiWithSql.Repositories;

public class CallAssignmentRepository: ICallAssignmentRepository
{
    private readonly string _connectionString;

    public CallAssignmentRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SqlServer")
            ?? throw new InvalidOperationException("Connection string was not found");
    }

    public void AddAssignment(int callId, int operatorId)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = @"INSERT INTO CallAssignments (CallId, OperatorId, AssignmentDate)
                    VALUES (@CallId, @OperatorId, @Date)";

        using var cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@CallId", callId);
        cmd.Parameters.AddWithValue("@OperatorId", operatorId);
        cmd.Parameters.AddWithValue("@Date", DateTime.UtcNow);

        cmd.ExecuteNonQuery();
    }

    public void RemoveAssignment(int callId, int operatorId)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = @"DELETE FROM CallAssignments 
                    WHERE CallId = @CallId AND OperatorId = @OperatorId";

        using var cmd = new SqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@CallId", callId);
        cmd.Parameters.AddWithValue("@OperatorId", operatorId);

        cmd.ExecuteNonQuery();
    }
}

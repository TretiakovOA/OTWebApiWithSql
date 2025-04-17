using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Interfaces.Services;

public interface ICallService
{
    Call? GetCall(int id);
    PaginatedResult<Call> GetCalls(int pageNumber, int pageSize);
    void UpdateCallDetails(int id, string result);
    void DeleteCall(int id);
    void AssignOperator(int callId, int operatorId);
    void UnassignOperator(int callId, int operatorId);
}

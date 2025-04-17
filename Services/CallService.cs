using OTWebApiWithSql.Interfaces.Repositories;
using OTWebApiWithSql.Interfaces.Services;
using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Services;

public class CallService: ICallService
{
    private readonly ICallRepository _repository;
    private readonly ICallAssignmentRepository _assignmentRepository;

    public CallService(ICallRepository repository, ICallAssignmentRepository assignmentRepository)
    {
        _repository = repository;
        _assignmentRepository = assignmentRepository;
    }

    public Call? GetCall(int id)
    {
        return _repository.GetById(id);
    }

    public PaginatedResult<Call> GetCalls(int pageNumber, int pageSize)
    {
        var calls = _repository.GetAll(pageNumber, pageSize, out int totalCount);
        return new PaginatedResult<Call>
        {
            Items = calls,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public void UpdateCallDetails(int id, string result)
    {
        var call = _repository.GetById(id) ?? throw new KeyNotFoundException("Call not found");
        call.Result = result;
        _repository.Update(call);
    }

    public void DeleteCall(int id) 
    {
        if (_repository.GetById(id) == null) 
            throw new KeyNotFoundException("Call not found");
        _repository.Delete(id);
    }

    public void AssignOperator(int callId, int operatorId)
    {
        if (_repository.GetById(callId) == null)
            throw new KeyNotFoundException("Call not found");

        _assignmentRepository.AddAssignment(callId, operatorId);
    }

    public void UnassignOperator(int callId, int operatorId)
    {
        if (_repository.GetById(callId) == null)
            throw new KeyNotFoundException("Call not found");

        _assignmentRepository.RemoveAssignment(callId, operatorId);
    }
}

using OTWebApiWithSql.Interfaces.Repositories;
using OTWebApiWithSql.Interfaces.Services;
using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Services;

public class OperatorService: IOperatorService
{
    private readonly IOperatorRepository _repository;

    public OperatorService(IOperatorRepository repository)
    {
        _repository = repository;
    }

    public Operator? GetOperator(int id)
    {
        return _repository.GetById(id);
    }

    public PaginatedResult<Operator> GetOperators(int pageNumber, int pageSize)
    {
        var operators = _repository.GetAll(pageNumber, pageSize, out int totalCount);
        return new PaginatedResult<Operator>
        {
            Items = operators,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public int CreateOperator(Operator entity)
    {
        return _repository.Add(entity);
    }

    public void UpdateOperator(Operator entity)
    {
        if (_repository.GetById(entity.Id) == null)
            throw new KeyNotFoundException("Operator not found");
        _repository.Update(entity);
    }

    public void DeleteOperator(int id)
    {
        if (_repository.GetById(id) == null)
            throw new KeyNotFoundException("Operator not found");
        _repository.Delete(id);
    }
}

using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Interfaces.Services;

public interface IOperatorService
{
    Operator? GetOperator(int id);
    PaginatedResult<Operator> GetOperators(int pageNumber, int pageSize);
    int CreateOperator(Operator entity);
    void UpdateOperator(Operator entity);
    void DeleteOperator(int id);
}

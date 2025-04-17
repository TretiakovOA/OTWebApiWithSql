using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Interfaces.Repositories;

public interface IOperatorRepository
{
    Operator? GetById(int id);
    IEnumerable<Operator> GetAll(int page, int size, out int totalCount);
    int Add(Operator entity);
    void Update(Operator entity);
    void Delete(int id);
}

using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Interfaces.Repositories;

public interface ICallRepository
{
    Call? GetById(int id);
    IEnumerable<Call> GetAll(int pageNumber, int pageSize, out int totalCount);
    int Add(Call call);
    void Update(Call call);
    void Delete(int id);
}

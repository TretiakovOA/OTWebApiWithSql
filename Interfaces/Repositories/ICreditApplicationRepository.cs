using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Interfaces.Repositories;

public interface ICreditApplicationRepository
{
    CreditApplication? GetById(int id);
    IEnumerable<CreditApplication> GetAll(int pageNumber, int pageSize, out int totalCount);
    int Add(CreditApplication application);
    void Update(CreditApplication application);
    void Delete(int id);
}

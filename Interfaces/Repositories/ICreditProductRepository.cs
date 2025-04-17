using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Interfaces.Repositories;

public interface ICreditProductRepository
{
    CreditProduct? GetById(int id);
    IEnumerable<CreditProduct> GetAll(int pageNumber, int pageSize, out int totalCount);
    int Add(CreditProduct product);
    void Update(CreditProduct product);
    void Delete(int id);
}

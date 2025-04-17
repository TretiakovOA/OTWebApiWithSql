using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Interfaces.Services;

public interface ICreditProductService
{
    CreditProduct? GetProduct(int id);
    PaginatedResult<CreditProduct> GetProducts(int pageNumber, int pageSize);
    int CreateProduct(CreditProduct product);
    void UpdateProduct(CreditProduct product);
    void DeleteProduct(int id);
}

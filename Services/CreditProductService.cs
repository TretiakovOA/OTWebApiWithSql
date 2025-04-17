using OTWebApiWithSql.Interfaces.Repositories;
using OTWebApiWithSql.Interfaces.Services;
using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Services;

public class CreditProductService: ICreditProductService
{
    private readonly ICreditProductRepository _repository;

    public CreditProductService(ICreditProductRepository repository)
    {
        _repository = repository;
    }

    public CreditProduct? GetProduct(int id)
    {
        return _repository.GetById(id);
    }

    public PaginatedResult<CreditProduct> GetProducts(int pageNumber, int pageSize)
    {
        var products = _repository.GetAll(pageNumber, pageSize, out int totalCount);
        return new PaginatedResult<CreditProduct>
        {
            Items = products,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public int CreateProduct(CreditProduct product)
    {
        if (product.InterestRate <= 0)
            throw new ArgumentException("Interest rate must be positive");

        return _repository.Add(product);
    }

    public void UpdateProduct(CreditProduct product)
    {
        if (_repository.GetById(product.Id) == null)
            throw new KeyNotFoundException("Product not found");

        _repository.Update(product);
    }

    public void DeleteProduct(int id)
    {
        if (_repository.GetById(id) == null)
            throw new KeyNotFoundException("Product not found");

        _repository.Delete(id);
    }
}

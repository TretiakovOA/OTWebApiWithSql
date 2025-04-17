using OTWebApiWithSql.Interfaces.Repositories;
using OTWebApiWithSql.Interfaces.Services;
using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Services;

public class CreditApplicationService: ICreditApplicationService
{
    private readonly ICreditApplicationRepository _applicationRepo;
    private readonly ICallRepository _callRepo;
    private readonly ICreditProductRepository _productRepo;

    public CreditApplicationService(
        ICreditApplicationRepository applicationRepo,
        ICallRepository callRepo,
        ICreditProductRepository productRepo)
    {
        _applicationRepo = applicationRepo;
        _callRepo = callRepo;
        _productRepo = productRepo;
    }

    public CreditApplication? GetApplication(int id)
    {
        return _applicationRepo.GetById(id);
    }

    public PaginatedResult<CreditApplication> GetApplications(int pageNumber, int pageSize)
    {
        var applications = _applicationRepo.GetAll(pageNumber, pageSize, out int totalCount);
        return new PaginatedResult<CreditApplication>
        {
            Items = applications,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public int CreateApplication(CreditApplication application)
    {
        if (_productRepo.GetById(application.CreditProductId) == null) 
            throw new ArgumentException("Invalid credit product");

        var appId = _applicationRepo.Add(application);

        // Создание связанного звонка
        _callRepo.Add(new Call
        {
            ScheduledDate = DateTime.UtcNow.AddHours(24),
            Result = string.Empty,
            IsProcessed = false
        });

        return appId;
    }

    public void UpdateApplication(CreditApplication application)
    {
        if (_applicationRepo.GetById(application.Id) == null)
            throw new KeyNotFoundException("Application not found");

        _applicationRepo.Update(application);
    }

    public void DeleteApplication(int id)
    {
        if (_applicationRepo.GetById(id) == null)
            throw new KeyNotFoundException("Application not found");

        _applicationRepo.Delete(id);
    }
}

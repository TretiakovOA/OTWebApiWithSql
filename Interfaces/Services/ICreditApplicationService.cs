using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Interfaces.Services;

public interface ICreditApplicationService
{
    CreditApplication? GetApplication(int id);
    PaginatedResult<CreditApplication> GetApplications(int pageNumber, int pageSize);
    int CreateApplication(CreditApplication application);
    void UpdateApplication(CreditApplication application);
    void DeleteApplication(int id);
}

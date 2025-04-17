using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Interfaces.Services;

public interface IClientService
{
    Client? GetClient(int id);
    PaginatedResult<Client> GetClients(int pageNumber, int pageSize);
    int CreateClient(Client client);
    void UpdateClient(Client client);
    void DeleteClient(int id);
}

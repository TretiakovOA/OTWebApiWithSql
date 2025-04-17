using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Interfaces.Repositories;

public interface IClientRepository
{
    Client? GetById(int id);
    IEnumerable<Client> GetAll(int pageNumber, int pageSize, out int totalCount);
    int Add(Client client);
    void Update(Client client);
    void Delete(int id);
}

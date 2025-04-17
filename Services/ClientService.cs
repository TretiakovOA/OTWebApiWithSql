using OTWebApiWithSql.Interfaces.Repositories;
using OTWebApiWithSql.Interfaces.Services;
using OTWebApiWithSql.Models;

namespace OTWebApiWithSql.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _repository;

    public ClientService(IClientRepository repository)
    {
        _repository = repository;
    }

    public Client? GetClient(int id)
    {
        return _repository.GetById(id);
    }

    public PaginatedResult<Client> GetClients(int pageNumber, int pageSize)
    {
        var clients = _repository.GetAll(pageNumber, pageSize, out int totalCount);
        return new PaginatedResult<Client>
        {
            Items = clients,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public int CreateClient(Client client)
    {
        if (client.Age < 0)
            throw new ArgumentException("Incorrect age of client");

        return _repository.Add(client);
    }

    public void UpdateClient(Client client)
    {
        if (_repository.GetById(client.Id) == null) 
            throw new KeyNotFoundException("Client not found");
        _repository.Update(client);
    }

    public void DeleteClient(int id)
    {
        if (_repository.GetById(id) == null)
            throw new KeyNotFoundException("Client not found");

        _repository.Delete(id);
    }
}

using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class ClientService
{
    private readonly AppDbContext _db;

    public ClientService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Client>> GetClients(string username)
    {
        return await _db.Clients
            .Where(c => c.User.Username == username)
            .ToListAsync();
    }

    public async Task<Client> CreateClient(Client client, string username)
    {
        var user = await _db.Users.FirstAsync(u => u.Username == username);
        client.UserId = user.Id;
        _db.Clients.Add(client);
        await _db.SaveChangesAsync();
        return client;
    }

    public async Task<Client> UpdateClient(int id, Client updated, string username)
    {
        var client = await _db.Clients
            .FirstOrDefaultAsync(c => c.Id == id && c.User.Username == username)
            ?? throw new Exception("Client not found");

        client.Name = updated.Name;
        client.Afm = updated.Afm;
        client.Address = updated.Address;
        client.City = updated.City;
        client.Country = updated.Country;

        await _db.SaveChangesAsync();
        return client;
    }

    public async Task DeleteClient(int id, string username)
    {
        var client = await _db.Clients
            .FirstOrDefaultAsync(c => c.Id == id && c.User.Username == username)
            ?? throw new Exception("Client not found");

        _db.Clients.Remove(client);
        await _db.SaveChangesAsync();
    }
}

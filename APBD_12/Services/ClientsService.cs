using APBD_12.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_12.Services;

public class ClientsService : IClientsService
{
    private readonly APBD12Context _context;

    public ClientsService(APBD12Context context)
    {
        _context = context;
    }
    
    public async Task<bool> DeleteClientAsync(int idClient)
    {
        var client = await _context.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == idClient);

        if (client == null)
        {
            throw new KeyNotFoundException("Client not found.");
        }

        if (client.ClientTrips.Any())
        {
            throw new InvalidOperationException("Client is assigned to at least one trip and cannot be deleted.");
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

        return true;
    }

    
}
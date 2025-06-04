using APBD_12.Models;
using APBD_12.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace APBD_12.Services;

public class TripsService : ITripsService
{
    private readonly APBD12Context _context;

    public TripsService(APBD12Context context)
    {
        _context = context;
    }

    public async Task<PageDTO> GetTripsAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var totalTrips = await _context.Trips.CountAsync();
        var totalPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

        var trips = await _context.Trips
            .Include(t => t.IdCountries)
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => MapToTripDTO(t))
            .ToListAsync();

        return new PageDTO
        {
            PageNum = page,
            PageSize = pageSize,
            AllPages = totalPages,
            Trips = trips
        };
    }

    private static TripDTO MapToTripDTO(Trip t)
    {
        return new TripDTO
        {
            Name = t.Name,
            Description = t.Description,
            DateFrom = t.DateFrom,
            DateTo = t.DateTo,
            MaxPeople = t.MaxPeople,
            Countries = t.IdCountries.Select(c => new CountryDTO
            {
                Name = c.Name
            }).ToList(),
            Clients = t.ClientTrips.Select(ct => new ClientDTO
            {
                FirstName = ct.IdClientNavigation.FirstName,
                LastName = ct.IdClientNavigation.LastName
            }).ToList()
        };
    }
    
    
    public async Task AssignClientToTripAsync(int idTrip, ClientAssignedTripDTO dto)
    {
        var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == dto.Pesel);
        if (existingClient != null)
            throw new Exception("Client with the given PESEL already exists.");

        var trip = await _context.Trips
            .Include(t => t.ClientTrips)
            .FirstOrDefaultAsync(t => t.IdTrip == idTrip);

        if (trip == null)
            throw new KeyNotFoundException("Trip not found.");

        if (trip.DateFrom < DateTime.Now)
            throw new InvalidOperationException("Cannot register for a past trip.");

        var newClient = new Client
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Telephone = dto.Telephone,
            Pesel = dto.Pesel
        };

        await _context.Clients.AddAsync(newClient);
        await _context.SaveChangesAsync(); 

        var clientTrip = new ClientTrip
        {
            IdClient = newClient.IdClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = dto.PaymentDate
        };

        await _context.ClientTrips.AddAsync(clientTrip);
        await _context.SaveChangesAsync();
    }
    
}
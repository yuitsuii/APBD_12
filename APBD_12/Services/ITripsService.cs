using APBD_12.Models.DTOs;

namespace APBD_12.Services;

public interface ITripsService
{
    Task<PageDTO> GetTripsAsync(int page, int pageSize);
    Task AssignClientToTripAsync(int idTrip, ClientAssignedTripDTO dto);
}
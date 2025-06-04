namespace APBD_12.Services;

public interface IClientsService
{
    Task<bool> DeleteClientAsync(int idClient);
}
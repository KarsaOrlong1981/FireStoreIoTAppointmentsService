using FireStoreIoTAppointmentsService.Models;

namespace FireStoreIoTAppointmentsService.Services
{
    public interface IClientService
    {
        Task<List<Clients>> GetAllClients();
        Task<Clients> GetClientByIDAsync(string id);
        Task SetNewClient(Clients client);
        Task OnTokenRefresh(Clients client);
    }
}

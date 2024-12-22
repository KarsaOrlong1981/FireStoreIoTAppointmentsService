using FireStoreIoTAppointmentsService.Models;
using IoTAppointmentsService.Database;
using Microsoft.EntityFrameworkCore;

namespace FireStoreIoTAppointmentsService.Services
{
    public class ClientService : IClientService
    {
        private readonly AppointmentsDbContext _dbContext;

        public ClientService()
        {
            _dbContext = new AppointmentsDbContext();
            _dbContext.Database.EnsureCreated();
        }

        public async Task<List<Clients>> GetAllClients()
        {
            return await _dbContext.Clients.ToListAsync();
        }

        public async Task<Clients> GetClientByIDAsync(string id)
        {
            return await _dbContext.Clients.FindAsync(id);
        }

        public async Task OnTokenRefresh(Clients client)
        {
            var existingClient = await _dbContext.Clients.FindAsync(client.Id);
            if (existingClient != null)
            {
                existingClient.DeviceToken = client.DeviceToken;
                _dbContext.Clients.Update(existingClient);
            }
            else
            {
                await SetNewClient(client);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task SetNewClient(Clients client)
        {
            await _dbContext.Clients.AddAsync(client);
            await _dbContext.SaveChangesAsync();
        }
    }
}

namespace IoTAppointmentsService.Services
{
    public interface IFirebaseService
    {
        Task<string> SendNotificationAsync(string deviceToken, string title, string body);
        Task SendNotificationToAllClients(string title, string message);
    }
}

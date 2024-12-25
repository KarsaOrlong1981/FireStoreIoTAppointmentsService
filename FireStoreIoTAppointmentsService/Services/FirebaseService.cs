using FireStoreIoTAppointmentsService;
using FireStoreIoTAppointmentsService.Services;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace IoTAppointmentsService.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly string _firebaseUrl = "https://fcm.googleapis.com/v1/projects/fir-notificationsservice/messages:send";
        private readonly GoogleCredential _googleCredential;
        private readonly IClientService _clientService;

        public FirebaseService(IClientService clientService)
        {
            try
            {
                _clientService = clientService;
                _googleCredential = GoogleCredential.FromJson(Constants.ServiceAccountJson)
                .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");
            }
            catch (Exception ex) { }   
        }

        public async Task SendNotificationToAllClients(string title, string message)
        {
            try
            {
                var clients = await _clientService.GetAllClients();
                foreach (var client in clients)
                {
                    await SendNotificationAsync(client.DeviceToken, title, message);
                }
            }
            catch (Exception ex) { }
        }

        public async Task<string> SendNotificationAsync(string deviceToken, string title, string body)
        {
            try
            {
                var message = new
                {
                    message = new
                    {
                        token = deviceToken,
                        notification = new
                        {
                            title,
                            body
                        }
                    }
                };

                string jsonMessage = JsonConvert.SerializeObject(message);

                // OAuth2 Access Token 
                string token = await _googleCredential.UnderlyingCredential.GetAccessTokenForRequestAsync();

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PostAsync(_firebaseUrl, new StringContent(jsonMessage, Encoding.UTF8, "application/json"));

                return await response.Content.ReadAsStringAsync();
            }
            catch(Exception ex) { }
            return string.Empty;
        }
    }
}

using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using IoTAppointmentsService.Models;
using Google.Cloud.Firestore;

namespace IoTAppointmentsService.Services
{
    public class FirebaseService 
    {
        private FirestoreDb _firestoreDb;

        public FirebaseService(IConfiguration configuration)
        {
            string projectId = configuration["Firestore:ProjectId"];
            string keyFilePath = configuration["Firestore:KeyFilePath"];

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", keyFilePath);

            _firestoreDb = FirestoreDb.Create(projectId);
            // Firebase Admin SDK initialisieren
            //FirebaseApp.Create(new AppOptions()
            //{
            //    Credential = GoogleCredential.FromFile(configuration["Firebase:CredentialFilePath"])
            //});

            //_firestoreDb = FirestoreDb.Create(configuration["Firebase:ProjectId"]);
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointments()
        {
            var appointmentsRef = _firestoreDb.Collection("appointments");
            var snapshot = await appointmentsRef.GetSnapshotAsync();
            var appointments = new List<Appointment>();

            foreach (var document in snapshot.Documents)
            {
                appointments.Add(document.ConvertTo<Appointment>());
            }

            return appointments;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentByMonth(string member, int year, int month)
        {
            var appointmentsRef = _firestoreDb.Collection("appointments");
            var query = appointmentsRef.WhereEqualTo("Member", member)
                                       .WhereEqualTo("Year", year)
                                       .WhereEqualTo("Month", month);

            var snapshot = await query.GetSnapshotAsync();
            var appointments = new List<Appointment>();

            foreach (var document in snapshot.Documents)
            {
                appointments.Add(document.ConvertTo<Appointment>());
            }

            return appointments;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDay(string member, int year, int month, int day)
        {
            var appointmentsRef = _firestoreDb.Collection("appointments");
            var query = appointmentsRef.WhereEqualTo("Member", member)
                                       .WhereEqualTo("Year", year)
                                       .WhereEqualTo("Month", month)
                                       .WhereEqualTo("Day", day);

            var snapshot = await query.GetSnapshotAsync();
            var appointments = new List<Appointment>();

            foreach (var document in snapshot.Documents)
            {
                appointments.Add(document.ConvertTo<Appointment>());
            }

            return appointments;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByYear(string member, int year)
        {
            var appointmentsRef = _firestoreDb.Collection("appointments");
            var query = appointmentsRef.WhereEqualTo("Member", member)
                                       .WhereEqualTo("Year", year);

            var snapshot = await query.GetSnapshotAsync();
            var appointments = new List<Appointment>();

            foreach (var document in snapshot.Documents)
            {
                appointments.Add(document.ConvertTo<Appointment>());
            }

            return appointments;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsForMember(string member)
        {
            var appointmentsRef = _firestoreDb.Collection("appointments");
            var query = appointmentsRef.WhereEqualTo("Member", member);

            var snapshot = await query.GetSnapshotAsync();
            var appointments = new List<Appointment>();

            foreach (var document in snapshot.Documents)
            {
                appointments.Add(document.ConvertTo<Appointment>());
            }

            return appointments;
        }

        public async Task<bool> UpdateAppointment(Appointment appointment)
        {
            var appointmentRef = _firestoreDb.Collection("appointments").Document(appointment.Id);
            var snapshot = await appointmentRef.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return false;
            }

            await appointmentRef.SetAsync(appointment, SetOptions.MergeAll);
            return true;
        }

        public async Task AddAppointment(Appointment appointment)
        {
            var appointmentsRef = _firestoreDb.Collection("appointments");
            await appointmentsRef.AddAsync(appointment);
        }

        public async Task<bool> DeleteAppointment(string id)
        {
            var appointmentRef = _firestoreDb.Collection("appointments").Document(id);
            var snapshot = await appointmentRef.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return false;
            }

            await appointmentRef.DeleteAsync();
            return true;
        }

        public async Task DeleteAllAppointments()
        {
            var appointmentsRef = _firestoreDb.Collection("appointments");
            var snapshot = await appointmentsRef.GetSnapshotAsync();

            foreach (var document in snapshot.Documents)
            {
                await document.Reference.DeleteAsync();
            }
        }
    }
}

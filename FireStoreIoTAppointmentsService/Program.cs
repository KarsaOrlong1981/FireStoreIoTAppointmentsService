using FireStoreIoTAppointmentsService.Services;
using IoTAppointmentsService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
         .AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITodoService, TodoService>();
builder.Services.AddSingleton<IClientService, ClientService>();
builder.Services.AddSingleton<IFirebaseService,FirebaseService>();
builder.Services.AddSingleton<IAppointmentService, AppointmentService>();
var app = builder.Build();
app.UseCors("CorsPolicy");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHealthChecks("/health");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

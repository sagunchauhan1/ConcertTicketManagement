using Microsoft.EntityFrameworkCore;
using TicketService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TicketDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("EventService", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001");
});

builder.Services.AddHttpClient("VenueService", client =>
{
    client.BaseAddress = new Uri("https://localhost:5003");
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

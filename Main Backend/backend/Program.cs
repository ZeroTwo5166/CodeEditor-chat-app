

using backend;
using backend.Hubs;
using backend.Interfaces;
using backend.ModelsDTO;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR(); //SignalR service for editor and chat functionality
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IDictionary<string, ProjectDTO>>(opt => new Dictionary<string, ProjectDTO>()); //Adding IDICT services 
builder.Services.AddSingleton<IDictionary<string, UserRoomConnectionDTO>>(opt =>
    new Dictionary<string, UserRoomConnectionDTO>());

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ProjectRepository>();
builder.Services.AddScoped<DelegateProjectRepository>(); // Register DelegateProjectRepository with DI

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
         .AllowAnyHeader()
         .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<Hub>("/chat");


app.Run();


using backend.Hub;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<MonacoHub>();
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IDictionary<string,ProjectDto>>(opt => new Dictionary<string, ProjectDto>()); //DI service for monacoHub
            builder.Services.AddSingleton<IDictionary<string, UserRoomConnection>>(opt =>
                new Dictionary<string, UserRoomConnection>()); //DI service for monacoHub
            builder.Services.AddSingleton<IDictionary<string, UserChatConnection>>(opt =>
                new Dictionary<string, UserChatConnection>()); //DI service for chatHub
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                     .AllowAnyHeader()
                     .AllowCredentials()) ;

                //builder.AllowAnyOrigin()
                          // .AllowAnyMethod()
                          // .AllowAnyHeader());
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
            app.MapHub<MonacoHub>("/chat");
            app.MapHub<ChatHub>("/chathub");

            app.Run();
        }
    }
}

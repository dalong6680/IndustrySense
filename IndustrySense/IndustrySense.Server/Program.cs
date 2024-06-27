using System;
using System.Configuration;
using System.Net.WebSockets;
using System.Text;
using IndustrySense.Server.Controllers;
using IndustrySense.Server.Infrastructure.Repository;
using IndustrySense.Server.Infrastructure.TcpServer;
using IndustrySense.Server.Services;
using IndustrySense.Server.Services.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IndustrySense.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Database
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", true, true)
                .AddInMemoryCollection()
                .Build();
            builder.Services.AddDbContextFactory<DatabaseContext>(
                options =>
                {
                    options.UseMySQL(
                        configuration.GetConnectionString("DefaultConnection")
                            ?? throw new Exception("Database connection srting not found!")
                    );
                },
                ServiceLifetime.Transient
            );

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register Services
            builder.Services.AddScoped<IElectricDataService, ElectricDataService>();
            builder.Services.AddSingleton<ITcpServer>(provider =>
            {
                var tcpServer = new TcpServer(12345); // Replace with your desired port number
                return tcpServer;
            });

            builder.Services.AddHostedService<TcpServerHostedService>();

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowSpecificOrigins",
                    builder =>
                    {
                        builder
                            .WithOrigins("https://localhost:5173") // 替换为你的前端地址
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                );
            });

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowSpecificOrigins"); // 使用配置的 CORS 策略

            app.UseAuthorization();

            app.MapControllers();

            app.UseWebSockets(
                new WebSocketOptions { KeepAliveInterval = TimeSpan.FromSeconds(15) }
            );

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}

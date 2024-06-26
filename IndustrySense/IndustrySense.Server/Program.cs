using System.Net.WebSockets;
using System.Text;
using IndustrySense.Server.Controllers;
using IndustrySense.Server.Services;
using IndustrySense.Server.Services.Impl;

namespace IndustrySense.Server
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

            // Register Services
            builder.Services.AddScoped<IElectricDataService, ElectricDataService>();

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowSpecificOrigins",
                    builder =>
                    {
                        builder
                            .WithOrigins("https://localhost:5173") // �滻Ϊ���ǰ�˵�ַ
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

            app.UseCors("AllowSpecificOrigins"); // ʹ�����õ� CORS ����

            app.UseAuthorization();

            app.MapControllers();

            app.UseWebSockets(new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(15)
            });
            //app.UseMiddleware<WebSocketMiddleware>(); // ʹ�� WebSocket �м��

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}

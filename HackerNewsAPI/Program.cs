using HackerNewsAPI.Ioc;
using HackerNewsAPI.Middleware;
using Microsoft.OpenApi.Models;

namespace HackerNewsAPI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Add services to the container.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "HackerNewsAPI",
                    Description = "Hacker News API",
                    Version = "v1"
                });
            });
            builder.Services.AddOperationProvider();
            builder.Services.AddMemoryCache();
           


            // Configure the HTTP request pipeline.
            var app = builder.Build();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapControllers();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.Run();
        }
    }
}

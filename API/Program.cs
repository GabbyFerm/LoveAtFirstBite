using Application;
using Infrastructure;
using Infrastructure.Extensions;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();


            // Add services from Application and Infrastructure
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services.AddJwtAuthentication(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerWithJwt();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontendPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors("FrontendPolicy");

            app.UseHttpsRedirection();
            app.UseAuthentication(); // must come before UseAuthorization
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
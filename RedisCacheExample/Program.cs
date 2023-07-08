using Microsoft.EntityFrameworkCore;
using RedisCacheExample.Data;
using RedisCacheExample.Services;

namespace RedisCacheExample
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

            builder.Services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>(
                opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("SampleDbConnection"))
                );
            builder.Services.AddScoped<ICacheService,CacheService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
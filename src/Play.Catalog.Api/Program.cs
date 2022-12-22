using Microsoft.AspNetCore.Authentication.JwtBearer;
using Play.Catalog.Api.Security;
using Play.Catalog.Data.Entities;
using Play.Common.Data;
using Play.Common.Identity;
using Play.Common.MassTansit;
using Play.Common.Settings;

namespace Play.Catalog.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var allowedOriginSettingsKey = "AllowedOrigins";

        var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

        // Add services to the container


        builder.Services.AddMongoDb()
                        .AddMongoRepository<Item>("Items");

        builder.Services.AddMassTransitWithRabbitMq();

        builder.Services.AddJwtBearerAuthentication();

        builder.Services.ConfigureOptions<ConfigureAuthorizationOptions>()
                        .AddAuthorization();


        builder.Services.AddControllers(options =>
        {
            ///To solve conflict happen on using nameOf(ControllerAction)
            options.SuppressAsyncSuffixInActionNames = false;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors(corsBuilder =>
            {
                corsBuilder.WithOrigins(builder.Configuration[allowedOriginSettingsKey])
                            .AllowAnyHeader()
                            .AllowAnyMethod();
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }


}

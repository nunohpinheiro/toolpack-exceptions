namespace RestAPI;

using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RestAPI.CustomExceptions;
using ToolPack.Exceptions.Web.DependencyInjection;

public class Startup
{
    // TODO: Move to minimal Program configuration setup

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestAPI", Version = "v1" });
        });

        services.AddToolPackExceptions(options =>
        {
            options
                .Map<EnhanceYourCalmException>(new(StatusCode.ResourceExhausted, 420, "Enhance Your Calm"))
                .Map<TeaPotException>(new(StatusCode.Unknown, 418, "I'm a teapot"));
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestAPI v1"));
        }

        app.UseToolPackExceptionsMiddleware();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

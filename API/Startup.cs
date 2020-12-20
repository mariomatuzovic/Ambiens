using API.Helpers;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
{
  public class Startup
  {
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
      _configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
      // Ordering of services is not important
      services.AddScoped<IProductRepository, ProductRepository>();
      // Below we added GenericRepository to the service container so that we can inject list whenever we need it in our app 
      // We use typeof because we do not know the types that we are going to be using with this repository, not unit compile time
      services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
      // We need to add AutoMapper as a service and specify the location(the assembly where we've created our MappingProfile class) of where our mapping profiles are located 
      services.AddAutoMapper(typeof(MappingProfiles));
      services.AddControllers();
      services.AddDbContext<StoreContext>(x =>
        x.UseSqlite(_configuration.GetConnectionString("DefaultConnection")));
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      // Ordering of middlewares is important
      app.UseHttpsRedirection();

      app.UseRouting();

      // We need to configure our API server to serve static content
      // We added this as middleware just underneath app.UseRouting
      app.UseStaticFiles();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}

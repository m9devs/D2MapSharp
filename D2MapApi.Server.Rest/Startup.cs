using System.Text.Json.Serialization;

using D2MapApi.Core.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace D2MapApi.Server.Rest
{
    public class Startup
    {
        public Startup(IConfiguration p_configuration)
        {
            Configuration = p_configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection p_services)
        {
            p_services.AddControllers()
                .AddJsonOptions(p_options =>
            {
                p_options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            p_services.AddMemoryCache();
            p_services.RegisterCoreServices(Configuration);
        }

        public void Configure(IApplicationBuilder p_app, IWebHostEnvironment p_env)
        {
            if (p_env.IsDevelopment())
            {
                p_app.UseDeveloperExceptionPage();
            }

            p_app.UseHttpsRedirection();

            p_app.UseRouting();

            p_app.UseAuthorization();

            p_app.UseEndpoints(p_endpoints =>
            {
                p_endpoints.MapControllers();
            });
        }
    }
}

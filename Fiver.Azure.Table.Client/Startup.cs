using Fiver.Azure.Table.Client.OtherLayers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fiver.Azure.Table.Client
{
    public class Startup
    {
        public static IConfiguration Configuration;

        public Startup(
            IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(
            IServiceCollection services)
        {
            services.AddScoped<IAzureTableStorage<Movie>>(factory =>
            {
                return new AzureTableStorage<Movie>(
                    new AzureTableSettings(
                        storageAccount: Configuration["Table_StorageAccount"],
                        storageKey: Configuration["Table_StorageKey"],
                        tableName: Configuration["Table_TableName"]));
            });
            services.AddScoped<IMovieService, MovieService>();

            services.AddMvc();
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseMvcWithDefaultRoute();
        }
    }
}

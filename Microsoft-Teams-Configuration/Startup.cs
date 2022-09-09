using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Logging;
using MicrosoftTeams_Configuration_ASPCORE.Services;
using ServiceStack.Text;

namespace MicrosoftTeams_Configuration_ASPCORE
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; set; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<GraphClientServices>();
            //services.AddSingleton<ApiTokenInMemoryClient>();
            services.AddScoped<TeamsServices>();
            services.AddHttpClient();
            services.AddOptions();
            services.AddRazorPages();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            IdentityModelEventSource.ShowPII = true;
            if (env.IsProduction())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}

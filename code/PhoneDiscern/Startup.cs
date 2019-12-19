
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhoneDiscern.Domain;
using PhoneDiscern.IServices;
using PhoneDiscern.Model;
using PhoneDiscern.Repository;
using PhoneDiscern.Services;

namespace PhoneDiscern
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddOptions();
            services.Configure<AppSettings>(Configuration);
			services.AddSingleton<IOcrService, TesseractOcrService>();
			services.AddSingleton<IContext<MarkType>, MarkTypeContext>();
			services.AddSingleton<IRepository<MarkType>, MarkTypeRepository>();
			services.AddSingleton<IContext<UserMark>, UserMarkContext>();
			services.AddSingleton<IRepository<UserMark>, UserMarkRepository>();
			services.AddSingleton<IDataService, DataService>();
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //设置默认路由
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

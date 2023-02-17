using LACHONG_QLHC_WEB_API.Extensions;
using LACHONG_QLHC_WEB_API.Helpers;
using LACHONG_QLHC_WEB_Contracts.BanTinContracts;
using LACHONG_QLHC_WEB_Contracts.ChamCongContracts;
using LACHONG_QLHC_WEB_Contracts.ComboboxContracts;
using LACHONG_QLHC_WEB_Contracts.HomeContracts;
using LACHONG_QLHC_WEB_Contracts.HoSoContracts;
using LACHONG_QLHC_WEB_Contracts.NotificationContracts;
using LACHONG_QLHC_WEB_Contracts.RepoContracts;
using LACHONG_QLHC_WEB_ImportServices.AsposeCell;
using LACHONG_QLHC_WEB_Repository.BanTinRepository;
using LACHONG_QLHC_WEB_Repository.ChamCongRepository;
using LACHONG_QLHC_WEB_Repository.ComboboxRepository;
using LACHONG_QLHC_WEB_Repository.Context;
using LACHONG_QLHC_WEB_Repository.HomeRepository;
using LACHONG_QLHC_WEB_Repository.HoSoRepository;
using LACHONG_QLHC_WEB_Repository.NotificationRepository;
using LACHONG_QLHC_WEB_Repository.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LACHONG_QLHC_WEB_API
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
            services.ConfigureCors();

            services.AddSingleton<DapperContext>();

            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IHoSoRepository, HoSoRepository>();
            services.AddScoped<IHoSoRepository, HoSoRepository>();
            services.AddScoped<IHoSoImportRepository, HoSoImportService>();
            services.AddScoped<IBanTinRepository, BanTinRepository>();
            services.AddScoped<IComboboxRepository, ComboboxRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IChamCongRepository, ChamCongRepository>();
            services.AddScoped<IHomeRepository, HomeRepository>();

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.Converters.Add(new CustomJsonConverterForType());
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LACHONG_CTTNS_WEB_API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LACHONG_CTTNS_WEB_API v1"));
            }

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}

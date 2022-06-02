using Financas.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using Hangfire;
using Hangfire.SqlServer;
using Autofac;
using Financas.Ifood;

namespace Financas.HttpHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new IFoodModule());
            builder.RegisterModule(new RepositoriesModule());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FinancasDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            }, optionsLifetime: ServiceLifetime.Transient);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //services.AddTransient<IEstabelecimentoIfoodRepository, EstabelecimentoIfoodRepository>();
            //services.AddTransient<IPedidoIfoodRepository, PedidoIfoodRepository>();
            //services.AddTransient<IRequestLogRepository, RequestLogRepository>();
            //services.AddTransient<IIFoodClientWrapper, IFoodClientWrapper>();
            //services.AddTransient<IAcessoIfoodRepository, AcessoIfoodRepository>();
            //services.AddTransient<IIFoodService, IFoodService>();

            services.AddMemoryCache();

            services.AddHangfire(configuration =>
            {
                configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("Default"), new SqlServerStorageOptions()
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });
            });
            services.AddHangfireServer();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Financas", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Financas v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseHangfireDashboard();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using ChallengeViceri.Api.Extensions;
using ChallengeViceri.Domain.Entities;
using ChallengeViceri.Infrastructure.Data.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using System.Linq;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Annotations;

namespace ChallengeViceri.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var useInMemory = Configuration.GetValue<bool>("UseInMemoryDatabase");
            if (useInMemory)
            {
                services.AddDbContext<ChallengeViceriContext>(options =>
                    options.UseInMemoryDatabase("ChallengeViceriDb"));
            }
            else
            {
                services.AddDbContext<ChallengeViceriContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("Default")));
            }

            services.AddDIRepository();
            services.AddDIApplication();

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.SetMinimumLevel(LogLevel.Warning);
            });

            services.AddControllers();
            services.AddHttpContextAccessor();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.SetIsOriginAllowed(_ => true)
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials();
                    });
            });

            services.AddSwaggerExamplesFromAssemblyOf<Startup>();

            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                var domainXml = Path.Combine(AppContext.BaseDirectory, "ChallengeViceri.Domain.xml");
                if (File.Exists(domainXml)) c.IncludeXmlComments(domainXml);
                var applicationXml = Path.Combine(AppContext.BaseDirectory, "ChallengeViceri.Application.xml");
                if (File.Exists(applicationXml)) c.IncludeXmlComments(applicationXml);
                var infrastructureXml = Path.Combine(AppContext.BaseDirectory, "ChallengeViceri.Infrastructure.xml");
                if (File.Exists(infrastructureXml)) c.IncludeXmlComments(infrastructureXml);

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChallengeViceri API", Version = "v1" });
                c.AddServer(new OpenApiServer { Url = "https://localhost:5001", Description = "Local HTTPS" });
                c.AddServer(new OpenApiServer { Url = "http://localhost:5000", Description = "Local HTTP" });

                c.ExampleFilters();
                c.EnableAnnotations();
            });

            services.Configure<IISServerOptions>(options => options.AllowSynchronousIO = true);
            services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger, ChallengeViceriContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChallengeViceri API v1");
                c.DefaultModelsExpandDepth(-1);
                c.EnableFilter();
            });

            if (dbContext.Database.IsRelational())
            {
                dbContext.Database.Migrate();
            }

            if (!dbContext.Superpowers.Any())
            {
                dbContext.Superpowers.AddRange(new[]
                {
                    new Superpower { Name = "Força", Description = "Força sobre-humana" },
                    new Superpower { Name = "Velocidade", Description = "Mover-se em alta velocidade" },
                    new Superpower { Name = "Voo", Description = "Capacidade de voar" },
                    new Superpower { Name = "Invisibilidade", Description = "Ficar invisível" },
                    new Superpower { Name = "Telepatia", Description = "Ler e comunicar-se pela mente" },
                    new Superpower { Name = "Telecinese", Description = "Mover objetos com a mente" },
                    new Superpower { Name = "Super Resistência", Description = "Alta resistência a danos" },
                    new Superpower { Name = "Regeneração", Description = "Cura acelerada" },
                    new Superpower { Name = "Rajada de Energia", Description = "Disparos de energia" },
                    new Superpower { Name = "Controle do Tempo", Description = "Manipular a passagem do tempo" }
                });
                dbContext.SaveChanges();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowAll");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}


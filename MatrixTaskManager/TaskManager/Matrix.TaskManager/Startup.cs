using Matrix.TaskManager.Common.Configuration;
using Matrix.TaskManager.Contexts;
using Matrix.TaskManager.Interfaces;
using Matrix.TaskManager.Repository;
using Matrix.TaskManager.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace Matrix.TaskManager
{
	public class Startup
	{
		public string Version { get; set; }
		public Startup(IWebHostEnvironment env)
		{
			HostingEnvironment = env;
			var builder = new ConfigurationBuilder()
			  .SetBasePath(env.ContentRootPath)
			  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			  .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
			  .AddEnvironmentVariables();
			Configuration = builder.Build();

			Log.Logger = new LoggerConfiguration().Enrich.FromLogContext().ReadFrom.Configuration(Configuration).CreateLogger();

			Version = typeof(Startup).Assembly.GetName().Version.ToString();
		}


		public IConfiguration Configuration { get; }
		public IWebHostEnvironment HostingEnvironment;
		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors();
			services.AddControllers();
			services.AddMemoryCache();
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

			services.AddScoped<IAuthenticationService, AuthenticationService>();
			services.AddScoped<ITaskManagerRepository, TaskManagerRepository>();
			services.AddScoped<IEmailRepository, EmailRepository>();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			//services.AddSingleton<IAgentConfig, AgentConfiguration>();
			ConfigureDataContext(services);
			ConfigureJWT(services);

			ConfigureSwaggerGen(services);

		}

		private void ConfigureJWT(IServiceCollection services)
		{
			var appSettingsSection = Configuration.GetSection("AppSettings");
			services.Configure<AppSettings>(appSettingsSection);
			var appSettings = appSettingsSection.Get<AppSettings>();
			var key = Encoding.ASCII.GetBytes(appSettings.Secret);
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});
		}

		private void ConfigureSwaggerGen(IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				if (!HostingEnvironment.IsProduction())
				{
					c.SwaggerDoc("task-svc",
						new Microsoft.OpenApi.Models.OpenApiInfo
						{
							Title = $"Task Manager ({HostingEnvironment.EnvironmentName})",
							Version = Version
						});
					c.SwaggerDoc("authentication-svc",
						new Microsoft.OpenApi.Models.OpenApiInfo
						{
							Title = $"Authentication ({HostingEnvironment.EnvironmentName})",
							Version = Version
						});
				}
			});
		}

		private void ConfigureDataContext(IServiceCollection services)
		{
			services.AddDbContext<TaskManagerContext>(opts =>
					opts.UseSqlServer(Configuration["ConnectionString:TaskManagerDB"]));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
			ILoggerFactory loggerFactory, IConfiguration config)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();


			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.DocumentTitle = $"{env.EnvironmentName} {Version}";
				c.HeadContent = $"{env.EnvironmentName} {Version}";

				if (!HostingEnvironment.IsProduction())
				{
					c.SwaggerEndpoint("/swagger/task-svc/swagger.json", "Matrix Task Manager");
					c.SwaggerEndpoint("/swagger/authentication-svc/swagger.json", "Authentication");
				}
			});

			app.UseRouting();


			loggerFactory.AddSerilog();

			// global cors policy
			app.UseCors(x => x
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader());

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseHttpsRedirection();


			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}

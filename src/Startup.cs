using static System.IO.Directory;

using Baelor.Database;
using Baelor.Database.Repositories;
using Baelor.Models.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Baelor.Database.Repositories.Interfaces;
using SendGrid;
using SendGrid.Connections;
using SharpRaven.Core.Configuration;
using Microsoft.AspNetCore.Http;
using SharpRaven.Core;
using Microsoft.Extensions.Options;

namespace Baelor
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("config.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"config.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
			HostingEnvironment = env;
		}

		public static IConfigurationRoot Configuration { get; private set; }

        public static IHostingEnvironment HostingEnvironment { get; private set; }

		public void ConfigureServices(IServiceCollection services)
		{
			// Default to snake_case
			services.AddMvc()
				.AddJsonOptions(jo =>
				{
					jo.SerializerSettings.ContractResolver = new DefaultContractResolver()
					{
						NamingStrategy = new SnakeCaseNamingStrategy()
					};
				});

			// Add Configurations
			services.Configure<MongoOptions>(Configuration.GetSection("MongoDB"));
			services.Configure<RavenOptions>(Configuration.GetSection("RavenOptions"));

			// Add Mongo Repositories
			services.AddSingleton<MongoDatabase>();
			services.AddSingleton<IClientRepository, ClientRepository>();
			services.AddSingleton<IEmailVerificationRepository, EmailVerificationRepository>();
			services.AddSingleton<IUserRepository, UserRepository>();
			
			// Add SendGrid
			services.AddSingleton<SendGridClient>(
				new SendGridClient(
					new ApiKeyConnection(Configuration.GetSection("SendGrid").GetValue<string>("ApiKey"))));

			// Add RavenClient
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<IRavenClient, RavenClient>((s) => {
				var rc = new RavenClient(s.GetRequiredService<IOptions<RavenOptions>>(), s.GetRequiredService<IHttpContextAccessor>())
				{
					Environment = HostingEnvironment.EnvironmentName
				};

				rc.Tags.Add("Environment", HostingEnvironment.EnvironmentName);
				return rc;
			});
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			app.UseMvc();
		}

		public static void Main(string[] args)
		{
			var config = new ConfigurationBuilder()
				.AddCommandLine(args)
				.AddEnvironmentVariables(prefix: "ASPNETCORE_")
				.Build();

			var host = new WebHostBuilder()
				.UseConfiguration(config)
				.UseKestrel()
				.UseContentRoot(GetCurrentDirectory())
				.UseStartup<Startup>()
				.Build();

			host.Run();
		}
	}
}

using BaelorApi.Models.Database;
using BaelorApi.Models.Repositories;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Diagnostics.Entity;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Logging.Console;
using System;

namespace BaelorApi
{
	public class Startup
	{
		/// <summary>
		/// The <see cref="IConfiguration"/> that stores application variables from <see cref="config.json"/> as well as Enviroment Variables.
		/// </summary>
		public static IConfiguration Configuration { get; set; }

		/// <summary>
		/// Settings to run on application startup.
		/// </summary>
		/// <param name="env">The <see cref="IHostingEnvironment"/> the application is running on.</param>
		public Startup(IHostingEnvironment env)
		{
			// Setup configuration sources.
			Configuration = new Configuration()
				.AddJsonFile("config.json")
				.AddEnvironmentVariables();
		}

		/// <summary>
		/// Configure the application.
		/// </summary>
		/// <param name="app">The <see cref="IApplicationBuilder"/> to configure.</param>
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerfactory)
		{
			// log pls
			loggerfactory.AddConsole();

			// Enable the MVC framework
			app.UseMvc(routes =>
			{
				routes.MapRoute("Default", "{controller}/{action}/{id?}", new { controller = "Home", action = "Index" });
				routes.MapWebApiRoute("v0", "v0/{controller}/{id?}");
			});

			if (string.Equals(env.EnvironmentName, "Development", StringComparison.OrdinalIgnoreCase))
			{
				app.UseBrowserLink();
				app.UseErrorPage(ErrorPageOptions.ShowAll);
				app.UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);
			}
			else
			{
				// Add Error handling middleware which catches all application specific errors and
				// send the request to the following path or controller action.
				// TODO: Add error controller action
				app.UseErrorHandler("/Home/Error");
			}

			// Enable Asset Pipeline
			app.UseStaticFiles();
		}

		/// <summary>
		/// Configure services to the application.
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection"/> created by ASP to regsiter services too.</param>
		public void ConfigureServices(IServiceCollection services)
		{
			// Add the MVC Framework
			services.AddMvc();

			// Add the Entity Framework
			services.AddEntityFramework().AddSqlServer().AddDbContext<DatabaseContext>();

			// Add Entity Framework related repository's and context's to the scope
			services.AddScoped<IAlbumRepository, AlbumRepository>();
			services.AddScoped<ISongRepository, SongRepository>();
			services.AddScoped<IImageRepository, ImageRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IRateLimitRepository, RateLimitRepository>();
			services.AddScoped<DatabaseContext, DatabaseContext>();

			// Add the Web Api Framework 
			services.AddWebApiConventions();

			#region [ DEBUG ]

#if DEBUG

			//// Clear RateLimit data
			//using (var dbContext = new DatabaseContext())
			//{
			//	var rateLimitRepo = new RateLimitRepository(dbContext);
			//	foreach (var rateLimit in rateLimitRepo.GetAll)
			//		rateLimitRepo.SetRquestCount(rateLimit.Id, 0);
			//}

#endif

			#endregion
		}
	}
}

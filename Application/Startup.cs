using System.Globalization;

using Application.Data;
using Application.Infrastructure;

using Localization.Configuration;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public static void ConfigureServices(IServiceCollection services)
		{
			ConfigureServicesLocalization(services);

			services.AddScoped<MainJavaScriptWrapper>();

			services.AddRazorPages();
			services.AddServerSideBlazor();

			services.AddResumeData<Startup>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseSessionLocalization();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
			});
		}

		/// <summary>
		/// Настройка служб локализации.
		/// </summary>
		/// <param name="services">Коллекция служб приложения.</param>
		private static void ConfigureServicesLocalization(IServiceCollection services)
		{
			CultureInfo defaultCulture = new CultureInfo("en");
			CultureInfo[] supportedCultures = new[]
			{
				defaultCulture,
				new CultureInfo("ru")
			};
			services.AddSessionLocalization(supportedCultures, defaultCulture, "Resources");
		}
	}
}

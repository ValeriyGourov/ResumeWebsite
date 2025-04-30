using System.Globalization;

using Application.Components;
using Application.Data;
using Application.Infrastructure.JavaScriptModules.Shared;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

_ = builder.ConfigureLogging();

ConfigureServicesLocalization(builder.Services);

_ = builder.Services.AddScoped<MainLayoutJavaScriptModule>();

// Add services to the container.
_ = builder.Services
	.AddRazorComponents()
	.AddInteractiveServerComponents();

_ = builder.Services.AddResumeData();

WebApplication app = builder.Build();

_ = app.UseSessionLocalization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	_ = app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	_ = app.UseHsts();
}

_ = app.UseHttpsRedirection();
_ = app.UseAntiforgery();

_ = app.MapStaticAssets();

_ = app
	.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

await app.RunAsync().ConfigureAwait(false);

static void ConfigureServicesLocalization(IServiceCollection services)
{
	CultureInfo defaultCulture = new("en");
	CultureInfo[] supportedCultures =
	[
		defaultCulture,
		new CultureInfo("ru")
	];
	services.AddSessionLocalization(supportedCultures, defaultCulture, "Resources");
}

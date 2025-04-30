using System.Globalization;

using Application.Components;
using Application.Data;
using Application.Infrastructure.JavaScriptModules.Shared;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigureServicesLocalization(builder.Services);

builder.Services.AddScoped<MainLayoutJavaScriptModule>();

// Add services to the container.
builder.Services
	.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddResumeData();

WebApplication app = builder.Build();

app.UseSessionLocalization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();

app
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

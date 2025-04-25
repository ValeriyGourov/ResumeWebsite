using System.Globalization;

using Application.Data;
using Application.Infrastructure.JavaScriptModules.Shared;

using Toolbelt.Blazor.Extensions.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigureServicesLocalization(builder.Services);

builder.Services.AddScoped<MainLayoutJavaScriptModule>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHeadElementHelper();

builder.Services.AddResumeData();

WebApplication app = builder.Build();

app.UseSessionLocalization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

await app.RunAsync().ConfigureAwait(false);

static void ConfigureServicesLocalization(IServiceCollection services)
{
	CultureInfo defaultCulture = new("en");
	CultureInfo[] supportedCultures = new[]
	{
		defaultCulture,
		new CultureInfo("ru")
	};
	services.AddSessionLocalization(supportedCultures, defaultCulture, "Resources");
}

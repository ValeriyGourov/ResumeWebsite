using System.Globalization;

using Application.Data;
using Application.Infrastructure;

using Toolbelt.Blazor.Extensions.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigureServicesLocalization(builder.Services);

builder.Services.AddScoped<MainJavaScriptWrapper>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHeadElementHelper();

builder.Services.AddResumeData();
//builder.Services.AddResumeData<Startup>();

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

app.Run();

/// <summary>
/// Настройка служб локализации.
/// </summary>
/// <param name="services">Коллекция служб.</param>
static void ConfigureServicesLocalization(IServiceCollection services)
{
	CultureInfo defaultCulture = new CultureInfo("en");
	CultureInfo[] supportedCultures = new[]
	{
		defaultCulture,
		new CultureInfo("ru")
	};
	services.AddSessionLocalization(supportedCultures, defaultCulture, "Resources");
}
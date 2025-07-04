using System.Globalization;

using Application.Components;
using Application.Data;
using Application.Infrastructure.JavaScriptModules.Shared;
using Application.Services.PdfGeneration;

using QuestPDF.Infrastructure;

using SoloX.BlazorJsBlob;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

_ = builder.ConfigureLogging();

ConfigureServicesLocalization(builder.Services);

_ = builder.Services.AddScoped<MainLayoutJavaScriptModule>();

// Add services to the container.
_ = builder.Services
	.AddRazorComponents()
	.AddInteractiveServerComponents();

_ = builder.Services.AddResumeData();

_ = builder.Services.AddJsBlob();

_ = builder.Services
	.AddScoped<IPdfGenerator, PdfGenerator>()
	.AddScoped<IDocument, PdfDocument>();

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

	_ = services.AddSessionLocalization(supportedCultures, defaultCulture, "Resources");
}

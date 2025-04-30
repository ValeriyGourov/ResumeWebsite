#pragma warning disable IDE0130 // Пространство имен (namespace) не соответствует структуре папок.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using CommunityToolkit.Diagnostics;

using Serilog;
using Serilog.Configuration;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Sinks.SystemConsole.Themes;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Методы расширения для настройки ведения журнала.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class LoggingExtensions
{
	/// <summary>
	/// Настройка параметров средства ведения журнала.
	/// </summary>
	/// <param name="hostApplicationBuilder">
	/// <inheritdoc cref="IHostApplicationBuilder" path="/summary"/>
	/// </param>
	/// <returns>
	/// Тот же экземпляр <see cref="IHostApplicationBuilder"/> для построения цепочки.
	/// </returns>
	/// <exception cref="InvalidOperationException">
	/// Не настроены все необходимые службы.
	/// </exception>
	public static IHostApplicationBuilder ConfigureLogging(this IHostApplicationBuilder hostApplicationBuilder)
	{
		Guard.IsNotNull(hostApplicationBuilder);

		// Bootstrap logger.
		LoggerConfiguration loggerConfiguration = new();
		ConfigureBasicEnrich(loggerConfiguration.Enrich);
		ConfigureWriteTo(loggerConfiguration.WriteTo);
		Log.Logger = loggerConfiguration.CreateBootstrapLogger();

		// Final logger.
		_ = hostApplicationBuilder.Services
			.AddSerilog(static (services, loggerConfiguration) =>
			{
				ConfigureEnrich(loggerConfiguration.Enrich);
				ConfigureWriteTo(loggerConfiguration.WriteTo);

				_ = loggerConfiguration
					.ReadFrom.Configuration(services.GetRequiredService<IConfiguration>())
					.ReadFrom.Services(services);
			});

		return hostApplicationBuilder;
	}

	/// <summary>
	/// Настраивает получателей событий журнала, используемых всеми поставщиками
	/// ведения журнала.
	/// </summary>
	/// <param name="writeTo">
	/// <inheritdoc cref="LoggerConfiguration.WriteTo" path="/summary"/>
	/// </param>
	private static void ConfigureWriteTo(LoggerSinkConfiguration writeTo)
	{
		const string mainOutputTemplate = "[{Timestamp:HH:mm:ss} {Level}] {Message:lj}";
		IFormatProvider formatProvider = CultureInfo.InvariantCulture;

		_ = writeTo.Debug(
			outputTemplate: mainOutputTemplate + " {Properties:j}{NewLine}{Exception}",
			formatProvider: formatProvider);
		_ = writeTo.Console(
			outputTemplate: mainOutputTemplate + "{NewLine}{Exception}",
			formatProvider: formatProvider,
			theme: AnsiConsoleTheme.Code);
	}

	/// <summary>
	/// Настраивает основных обогатителей событий журнала, используемых всеми поставщиками
	/// ведения журнала.
	/// </summary>
	/// <param name="enrich">
	/// <inheritdoc cref="LoggerConfiguration.Enrich" path="/summary"/>
	/// </param>
	private static void ConfigureBasicEnrich(LoggerEnrichmentConfiguration enrich)
		=> _ = enrich.FromLogContext();

	/// <summary>
	/// Настраивает обогатителей событий журнала основного поставщика ведения журнала.
	/// </summary>
	/// <param name="enrich">
	/// <inheritdoc cref="LoggerConfiguration.Enrich" path="/summary"/>
	/// </param>
	private static void ConfigureEnrich(LoggerEnrichmentConfiguration enrich)
	{
		ConfigureBasicEnrich(enrich);

		_ = enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
			.WithDefaultDestructurers());
	}
}

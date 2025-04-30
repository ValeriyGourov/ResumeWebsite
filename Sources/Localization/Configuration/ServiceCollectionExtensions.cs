#pragma warning disable IDE0130

using System.Globalization;

using CommunityToolkit.Diagnostics;

using Localization.Infrastructure;
using Localization.Infrastructure.JavaScriptModules;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Методы расширения для <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Добавляет возможность переключения культуры приложения в рамках одной сессии без
	/// перезагрузки приложения или обновления страницы.
	/// </summary>
	/// <param name="services">Коллекция служб приложения.</param>
	/// <param name="supportedCultures">Поддерживаемые приложением культуры.</param>
	/// <param name="defaultCulture">
	/// Культура по умолчанию, используемая когда невозможно определить требуемую культуру из
	/// списка поддерживаемых культур.
	/// </param>
	/// <param name="resourcesPath">
	/// Путь относительно корня приложения, по которому расположены файлы ресурсов.
	/// </param>
	/// <returns>Коллекция служб приложения.</returns>
	public static IServiceCollection AddSessionLocalization(
		this IServiceCollection services,
		CultureInfo[] supportedCultures,
		CultureInfo defaultCulture,
		string? resourcesPath = null)
	{
		Guard.IsNotNull(services);

		services.AddLocalization(options =>
		{
			if (!string.IsNullOrWhiteSpace(resourcesPath))
			{
				options.ResourcesPath = resourcesPath;
			}
		});

		services.Configure<RequestLocalizationOptions>(options =>
		{
			options.DefaultRequestCulture = new RequestCulture(defaultCulture);
			options.SupportedCultures = supportedCultures;
			options.SupportedUICultures = supportedCultures;
		});

		services
			.AddScoped<LocalizationJavaScriptModule>()
			.AddSingleton<CultureChanger>()
			.AddSingleton<IStringLocalizerFactory, SessionCultureResourceManagerStringLocalizerFactory>();

		return services;
	}
}

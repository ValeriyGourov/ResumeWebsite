#pragma warning disable IDE0130
#pragma warning disable CA1034 // TODO: Удалить после исправления ошибки анализатора: https://github.com/dotnet/roslyn-analyzers/issues/7765

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
	extension(IServiceCollection services)
	{
		/// <summary>
		/// Добавляет возможность переключения культуры приложения в рамках одной сессии без
		/// перезагрузки приложения или обновления страницы.
		/// </summary>
		/// <param name="supportedCultures">Поддерживаемые приложением культуры.</param>
		/// <param name="defaultCulture">
		/// Культура по умолчанию, используемая когда невозможно определить требуемую культуру из
		/// списка поддерживаемых культур.
		/// </param>
		/// <param name="resourcesPath">
		/// Путь относительно корня приложения, по которому расположены файлы ресурсов.
		/// </param>
		/// <returns>Коллекция служб приложения.</returns>
		public IServiceCollection AddSessionLocalization(
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
}

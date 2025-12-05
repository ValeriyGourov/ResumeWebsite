#pragma warning disable IDE0130
#pragma warning disable CA1034 // TODO: Удалить после исправления ошибки анализатора: https://github.com/dotnet/roslyn-analyzers/issues/7765

using CommunityToolkit.Diagnostics;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Методы расширения для <see cref="IApplicationBuilder"/>.
/// </summary>
public static class ApplicationBuilderExtensions
{
	extension(IApplicationBuilder app)
	{
		/// <summary>
		/// Включает возможность переключения культуры приложения в рамках одной сессии без
		/// перезагрузки приложения или обновления страницы.
		/// </summary>
		/// <returns>Класс для конфигурации конвеера запросов приложения.</returns>
		public IApplicationBuilder UseSessionLocalization()
		{
			Guard.IsNotNull(app);

			IOptions<RequestLocalizationOptions> localizationOptions = app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>();
			app.UseRequestLocalization(localizationOptions.Value);

			return app;
		}
	}
}

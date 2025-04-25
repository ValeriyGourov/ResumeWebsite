#pragma warning disable IDE0130

using CommunityToolkit.Diagnostics;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Методы расширения для <see cref="IApplicationBuilder"/>.
/// </summary>
public static class ApplicationBuilderExtensions
{
	/// <summary>
	/// Включает возможность переключения культуры приложения в рамках одной сессии без
	/// перезагрузки приложения или обновления страницы.
	/// </summary>
	/// <param name="app">Класс для конфигурации конвеера запросов приложения.</param>
	/// <returns>Класс для конфигурации конвеера запросов приложения.</returns>
	public static IApplicationBuilder UseSessionLocalization(this IApplicationBuilder app)
	{
		Guard.IsNotNull(app);

		IOptions<RequestLocalizationOptions>? localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>()
			?? throw new ApplicationException($"Не определены параметры {nameof(RequestLocalizationOptions)} для ПО промежуточного слоя локализации");
		app.UseRequestLocalization(localizationOptions.Value);

		return app;
	}
}

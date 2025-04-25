using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderExtensions
{
	/// <summary>
	/// Включает возможность переключения культуры приложения в рамках одной сессии без перезагрузки приложения или обновления страницы.
	/// </summary>
	/// <param name="app">Класс для конфигурации конвеера запросов приложения.</param>
	/// <returns>Класс для конфигурации конвеера запросов приложения.</returns>
	public static IApplicationBuilder UseSessionLocalization(this IApplicationBuilder app)
	{
		if (app is null)
		{
			throw new ArgumentNullException(nameof(app));
		}

		IOptions<RequestLocalizationOptions>? localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>() ?? throw new ApplicationException($"Не определены параметры {nameof(RequestLocalizationOptions)} для ПО промежуточного слоя локализации");
		app.UseRequestLocalization(localizationOptions.Value);

		return app;
	}
}
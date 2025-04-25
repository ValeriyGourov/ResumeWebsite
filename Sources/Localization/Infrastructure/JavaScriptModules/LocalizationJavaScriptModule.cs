using CommunityToolkit.Diagnostics;

using Infrastructure;

using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Localization.Infrastructure.JavaScriptModules;

/// <summary>
/// Методы, вызывающие функции JavaScript для механизмов локализации.
/// </summary>
internal sealed class LocalizationJavaScriptModule : JavaScriptModuleBase
{
	/// <summary>
	/// Конструктор на основании экземпляра среды выполнения JavaScript.
	/// </summary>
	/// <param name="logger">
	/// <inheritdoc
	///		cref="JavaScriptModuleBase.JavaScriptModuleBase(ILogger{JavaScriptModuleBase}, IJSRuntime, string)"
	///		path="/param[@name='logger']"/>
	///	</param>
	/// <param name="jsRuntime"><inheritdoc cref="IJSRuntime" path="/summary"/></param>
	/// <exception cref="ArgumentException">
	/// Не указано значение обязательного параметра.
	/// </exception>
	public LocalizationJavaScriptModule(ILogger<LocalizationJavaScriptModule> logger, IJSRuntime jsRuntime)
		: base(logger, jsRuntime, "./_content/Localization/js/localization.js")
	{ }

	/// <summary>
	/// Создаёт куки-файл в системе пользователя.
	/// </summary>
	/// <param name="name">Имя куки-файла.</param>
	/// <param name="value">Значение куки-файла.</param>
	/// <param name="expirationDays">Срок жизни куки-файла, исчисляемый в днях.</param>
#pragma warning disable VSTHRD200
	public ValueTask CreateCookie(string name, string value, int expirationDays)
#pragma warning restore VSTHRD200
	{
		Guard.IsNotNullOrWhiteSpace(name);
		Guard.IsNotNullOrWhiteSpace(value);

		return InvokeVoidAsync(new object[] { name, value, expirationDays });
	}
}

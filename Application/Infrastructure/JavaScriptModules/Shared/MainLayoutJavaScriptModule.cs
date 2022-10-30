using Infrastructure;

using Microsoft.JSInterop;

namespace Application.Infrastructure.JavaScriptModules.Shared;

/// <summary>
/// Методы, вызывающие функции JavaScript для компонента <see cref="Application.Shared.MainLayout"/>.
/// </summary>
internal sealed class MainLayoutJavaScriptModule : JavaScriptModuleBase
{
	/// <summary>
	/// Конструктор на основании экземпляра среды выполнения JavaScript.
	/// </summary>
	/// <param name="logger"><inheritdoc cref="JavaScriptModuleBase.JavaScriptModuleBase(ILogger{JavaScriptModuleBase}, IJSRuntime, string)" path="/param[@name='logger']"/></param>
	/// <param name="jsRuntime"><inheritdoc cref="IJSRuntime" path="/summary"/></param>
	/// <exception cref="ArgumentException">Не указано значение обязательного параметра.</exception>
	public MainLayoutJavaScriptModule(ILogger<MainLayoutJavaScriptModule> logger, IJSRuntime jsRuntime)
		: base(logger, jsRuntime, "./Shared/MainLayout.razor.js")
	{ }

	/// <summary>
	/// Показывает главный контейнер приложения и скрывает вращатель.
	/// </summary>
	public ValueTask ShowMainContainer() => InvokeVoidAsync();
}
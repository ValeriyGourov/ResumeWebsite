using Infrastructure;

using Microsoft.JSInterop;

namespace Application.Infrastructure.JavaScriptModules.Shared;

/// <summary>
/// Методы, вызывающие функции JavaScript для компонента
/// <see cref="Components.Layout.MainLayout"/>.
/// </summary>
/// <param name="logger">
/// <inheritdoc
///		cref="JavaScriptModuleBase(ILogger{JavaScriptModuleBase}, IJSRuntime, string)"
///		path="/param[@name='logger']"/>
/// </param>
/// <param name="jsRuntime"><inheritdoc cref="IJSRuntime" path="/summary"/></param>
/// <exception cref="ArgumentException">
/// Не указано значение обязательного параметра.
/// </exception>
internal sealed class MainLayoutJavaScriptModule(
	ILogger<MainLayoutJavaScriptModule> logger,
	IJSRuntime jsRuntime)
	: JavaScriptModuleBase(logger, jsRuntime, "./Components/Layout/MainLayout.razor.js")
{
	/// <summary>
	/// Показывает главный контейнер приложения и скрывает вращатель.
	/// </summary>
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
	public ValueTask ShowMainContainer() => InvokeVoidAsync();
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
}

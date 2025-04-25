using System.Runtime.CompilerServices;
using System.Text.Json;

using CommunityToolkit.Diagnostics;

using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Infrastructure;

/// <summary>
/// Базовый класс для классов-обёрток вызова функций JavaScript.
/// </summary>
public abstract class JavaScriptModuleBase : IAsyncDisposable
{
	/// <summary>
	/// Средство ведения журнала.
	/// </summary>
	protected readonly ILogger _logger;

	/// <inheritdoc cref="IJSRuntime"/>
	private readonly IJSRuntime _jsRuntime;

	/// <inheritdoc cref="IJSObjectReference"/>
	/// <remarks>
	/// Не должен использоваться в явном виде. Вместо этого необходимо использовать метод <see cref="GetJSObjectReference(CancellationToken)"/>.
	/// </remarks>
	private IJSObjectReference? _jsObjectReference;

	/// <summary>
	/// Пусть к файлу JavaScript-модуля относительно папки wwwroot в формате "./{SCRIPT PATH AND FILENAME (.js)}".
	/// </summary>
	protected string _scriptPath;

	/// <summary>
	/// Конструктор на основании экземпляра среды выполнения JavaScript и пути к файлу JavaScript-модуля.
	/// </summary>
	/// <param name="logger"><inheritdoc cref="_logger" path="/summary"/></param>
	/// <param name="jsRuntime"><inheritdoc cref="IJSRuntime" path="/summary"/></param>
	/// <param name="scriptPath"><inheritdoc cref="_scriptPath" path="/summary"/></param>
	/// <exception cref="ArgumentException">Не указано значение обязательного параметра.</exception>
	/// <exception cref="ArgumentNullException">Не указано значение обязательного параметра.</exception>
	protected JavaScriptModuleBase(
		ILogger<JavaScriptModuleBase> logger,
		IJSRuntime jsRuntime,
		string scriptPath)
	{
		Guard.IsNotNull(logger);
		Guard.IsNotNull(jsRuntime);
		Guard.IsNotNullOrWhiteSpace(scriptPath);

		_logger = logger;
		_jsRuntime = jsRuntime;
		_scriptPath = scriptPath;
	}

	/// <summary>
	/// Возвращает (при необходимости создаёт) ссылку на объект JavaScript, используемый для вызова методов в JavaScript-модуле.
	/// </summary>
	/// <param name="cancellationToken">Токен отмены для сигнализации об отмене операции.</param>
	/// <returns>Ссылка на объект JavaScript.</returns>
	protected async ValueTask<IJSObjectReference> GetJSObjectReference(CancellationToken cancellationToken = default) => _jsObjectReference ??= await _jsRuntime
		.InvokeAsync<IJSObjectReference>("import", cancellationToken, _scriptPath)
		.ConfigureAwait(true);

	/// <summary>
	/// Вызывает метод JavaScript-модуля по его идентификатору и с заданным набором параметров.
	/// </summary>
	/// <param name="identifier">Идентификатор вызываемого метода.</param>
	/// <param name="args">Параметры вызываемого метода.</param>
	/// <param name="cancellationToken">Токен отмены для сигнализации об отмене операции.</param>
	protected async ValueTask InvokeVoidAsync(
		string identifier,
		object?[]? args = null,
		CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Данные: {Identifier}, {@Args}", identifier, args);

		IJSObjectReference jsObjectReference = await GetJSObjectReference(cancellationToken).ConfigureAwait(true);
		await jsObjectReference
			.InvokeVoidAsync(identifier, cancellationToken, args)
			.ConfigureAwait(true);
	}

	/// <summary>
	/// Вызывает метод JavaScript-модуля с заданным набором параметров. Идентификатор JavaScript-метода должен совпадать с именем метода модуля-обёртки, при этом применяется преобразование в camelCase.
	/// </summary>
	/// <param name="args"><inheritdoc cref="InvokeVoidAsync(string, object?[], CancellationToken)" path="/param[@name='args']"/></param>
	/// <param name="cancellationToken"><inheritdoc cref="InvokeVoidAsync(string, object?[], CancellationToken)" path="/param[@name='cancellationToken']"/></param>
	/// <param name="methodName">Имя метода модуля-обёртки. Всегда определяется автоматически и не должно указываться в явном виде.</param>
	protected ValueTask InvokeVoidAsync(
		object?[]? args = null,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string methodName = "")
	{
		_logger.LogDebug("Данные: {@Args}", args ?? Array.Empty<object?[]>());

		return InvokeVoidAsync(ConvertMethodName(methodName), args, cancellationToken);
	}

	/// <summary>
	/// Вызывает метод JavaScript-модуля по его идентификатору и с заданным набором параметров, возвращает значение указанного типа.
	/// </summary>
	/// <typeparam name="TValue">Тип возвращаемого значения.</typeparam>
	/// <param name="identifier">Идентификатор вызываемого метода.</param>
	/// <param name="args">Параметры вызываемого метода.</param>
	/// <param name="cancellationToken">Токен отмены для сигнализации об отмене операции.</param>
	/// <returns>Результат вызова метода.</returns>
	protected async ValueTask<TValue> InvokeAsync<TValue>(
		string identifier,
		object?[]? args = null,
		CancellationToken cancellationToken = default)
	{
		_logger.LogDebug("Данные: {Identifier}, {@Args}", identifier, args);

		IJSObjectReference jsObjectReference = await GetJSObjectReference(cancellationToken).ConfigureAwait(true);
		return await jsObjectReference
			.InvokeAsync<TValue>(identifier, cancellationToken, args)
			.ConfigureAwait(true);
	}

	/// <summary>
	/// Вызывает метод JavaScript-модуля с заданным набором параметров и возвращает значение указанного типа. Идентификатор JavaScript-метода должен совпадать с именем метода модуля-обёртки, при этом применяется преобразование в camelCase.
	/// </summary>
	/// <typeparam name="TValue">Тип возвращаемого значения.</typeparam>
	/// <param name="args"><inheritdoc cref="InvokeAsync(string, object?[], CancellationToken)" path="/param[@name='args']"/></param>
	/// <param name="cancellationToken"><inheritdoc cref="InvokeAsync(string, object?[], CancellationToken)" path="/param[@name='cancellationToken']"/></param>
	/// <param name="methodName">Имя метода модуля-обёртки. Всегда определяется автоматически и не должно указываться в явном виде.</param>
	/// <returns><inheritdoc cref="InvokeAsync(string, object?[], CancellationToken)" path="/returns"/></returns>
	protected ValueTask<TValue> InvokeAsync<TValue>(
		object?[]? args = null,
		CancellationToken cancellationToken = default,
		[CallerMemberName] string methodName = "")
	{
		_logger.LogDebug("Данные: {@Args}", args ?? Array.Empty<object?[]>());

		return InvokeAsync<TValue>(ConvertMethodName(methodName), args, cancellationToken);
	}

	/// <summary>
	/// Преобразовывает имя метода для представления его в виде camelCase.
	/// </summary>
	/// <param name="methodName">Исходное имя метода.</param>
	/// <returns>Имя метода, преобразованное в camelCase.</returns>
	private static string ConvertMethodName(string methodName) => JsonNamingPolicy.CamelCase.ConvertName(methodName);

	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore().ConfigureAwait(false);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Асинхронно выполняет освобождение ресурсов.
	/// </summary>
	protected virtual async ValueTask DisposeAsyncCore()
	{
		if (_jsObjectReference is not null)
		{
			try
			{
				await _jsObjectReference
					.DisposeAsync()
					.ConfigureAwait(false);
			}
			catch (JSDisconnectedException exception)
			{
				// В данном случае игнорируем это исключение.

				_logger.LogWarning(
					exception,
					"При освобождении ресурсов возникла ошибка: {ExceptionMessage}",
					exception.Message);
			}
		}

		_jsObjectReference = null!;
	}
}
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Localization.Infrastructure;
using Localization.Infrastructure.JavaScriptModules;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Threading;

namespace Localization.Components;

/// <summary>
/// Базовый класс компонента для переключения культуры приложения.
/// </summary>
/// <param name="logger">
/// <inheritdoc cref="Logger" path="/summary"/>
/// </param>
public abstract partial class CultureSwitcherBase(ILogger<CultureSwitcherBase> logger)
	: ComponentBase, System.IAsyncDisposable
{
	/// <summary>
	/// Срок хранения куки-файла с культурой, исчисляемый в днях.
	/// </summary>
	private const int _userCultureCookieExpirationDays = 180;

	/// <summary>
	/// Механизм для асинхронного выполнения обработчиков событий.
	/// </summary>
	private readonly JoinableTaskFactory _joinableTaskFactory = new(new JoinableTaskContext());

	/// <summary>
	/// Преобразователь культуры, предоставляющий сведения о используемой культуре приложения.
	/// </summary>
	[Inject] private CultureChanger CultureChanger { get; set; } = null!;

	/// <summary>
	/// Настройки локализации.
	/// </summary>
	[Inject] protected IOptions<RequestLocalizationOptions> RequestLocalizationOptions { get; set; } = null!;

	/// <summary>
	/// Модуль-обёртка для вызова функций JavaScript.
	/// </summary>
	[Inject] private LocalizationJavaScriptModule JSModule { get; set; } = null!;

	/// <summary>
	/// Средство ведения журнала.
	/// </summary>
	protected ILogger<CultureSwitcherBase> Logger { get; set; } = logger;

	/// <summary>
	/// Поддерживаемые культуры приложения.
	/// </summary>
	public IList<CultureInfo> SupportedUICultures => RequestLocalizationOptions.Value.SupportedUICultures;

	/// <inheritdoc/>
	protected override void OnInitialized()
	{
		CultureChanger.CultureChanged += CultureChanged;

		base.OnInitialized();
	}

	/// <summary>
	/// Обработчик события изменения выбранной культуры приложения. Выполняет запись куки-файла
	/// культуры в системе пользователя для использования при следующем запуске приложения.
	/// </summary>
	private void CultureChanged(object? sender, EventArgs e)
	{
		_ = _joinableTaskFactory.RunAsync(async () =>
		{
			string cookieName = CookieRequestCultureProvider.DefaultCookieName;
			string cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(CultureChanger.CurrentUICulture));

			LogInformationCreatingCookie(cookieName, cookieValue);
			await JSModule
				.CreateCookie(cookieName, cookieValue, _userCultureCookieExpirationDays)
				.ConfigureAwait(true);
		});
	}

	/// <summary>
	/// Устанавливает новые культуры приложения.
	/// </summary>
	/// <param name="culture">
	/// Объект, представляющий язык и региональные параметры, используемые текущим приложением.
	/// </param>
	/// <param name="uiCulture">
	/// Объект, представляющий текущий язык и региональные параметры пользовательского
	/// интерфейса, используемые диспетчером ресурсов для поиска ресурсов, связанных с
	/// конкретным языком и региональными параметрами, во время выполнения.
	/// </param>
	protected virtual void ChangeCulture(CultureInfo culture, CultureInfo uiCulture) => CultureChanger.ChangeCulture(culture, uiCulture);

	/// <summary>
	/// Устанавливает новые культуры приложения.
	/// </summary>
	/// <param name="culture">
	/// Объект, представляющий язык и региональные параметры, используемые текущим приложением.
	/// Это же значение используется для пользовательского интерфейса.
	/// </param>
	protected virtual void ChangeCulture(CultureInfo culture) => CultureChanger.ChangeCulture(culture, culture);

	/// <inheritdoc/>
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
		CultureChanger.CultureChanged -= CultureChanged;

		if (JSModule is not null)
		{
			await JSModule
				.DisposeAsync()
				.ConfigureAwait(false);
		}

		JSModule = null!;
	}

	#region Методы журналирования

	[ExcludeFromCodeCoverage]
	[LoggerMessage(
		Level = LogLevel.Information,
		Message = "Запись куки-файла '{CookieName}' со значением '{CookieValue}'.")]
	private partial void LogInformationCreatingCookie(string cookieName, string cookieValue);

	#endregion
}

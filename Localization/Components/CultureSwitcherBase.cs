using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using Localization.Infrastructure;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Localization.Components
{
	/// <summary>
	/// Базовый класс компонента для переключения культуры приложения.
	/// </summary>
	public abstract class CultureSwitcherBase : ComponentBase, IDisposable
	{
		/// <summary>
		/// Срок хранения куки-файла с культурой, исчисляемый в днях.
		/// </summary>
		private const int _userCultureCookieExpirationDays = 180;

		private bool _disposedValue;

		/// <summary>
		/// Преобразователь культуры, предоставляющий сведения о используемой культуре приложения.
		/// </summary>
		[Inject] private CultureChanger CultureChanger { get; set; } = null!;

		/// <summary>
		/// Настройки локализации.
		/// </summary>
		[Inject] protected IOptions<RequestLocalizationOptions> RequestLocalizationOptions { get; set; } = null!;

		/// <summary>
		/// Обёртка для вызова функций JavaScript.
		/// </summary>
		[Inject] private JavaScriptWrapper JSWrapper { get; set; } = null!;

		/// <summary>
		/// Локализатор строк.
		/// </summary>
		[Inject] private IStringLocalizer<CultureSwitcherBase> Localizer { get; set; } = null!;

		/// <summary>
		/// Регистратор событий.
		/// </summary>
		[Inject] protected ILogger<CultureSwitcherBase> Logger { get; set; } = null!;

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
		/// Обработчик события изменения выбранной культуры приложения. Выполняет запись куки-файла культуры в системе пользователя для использования при следующем запуске приложения.
		/// </summary>
		private Task CultureChanged()
		{
			string cookieName = CookieRequestCultureProvider.DefaultCookieName;
			string cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(CultureChanger.CurrentUICulture));
			Logger.LogInformation(Localizer["Запись куки-файла '{cookieName}' со значением '{cookieValue}'."], cookieName, cookieValue);

			return InvokeAsync(async () =>
			{
				await JSWrapper
					.CreateCookie(cookieName, cookieValue, _userCultureCookieExpirationDays)
					.ConfigureAwait(true);
			});
		}

		/// <summary>
		/// Устанавливает новые культуры приложения.
		/// </summary>
		/// <param name="culture">Объект, представляющий язык и региональные параметры, используемые текущим приложением.</param>
		/// <param name="uiCulture">Объект, представляющий текущий язык и региональные параметры пользовательского интерфейса, используемые диспетчером ресурсов для поиска ресурсов, связанных с конкретным языком и региональными параметрами, во время выполнения.</param>
		protected virtual void ChangeCulture(CultureInfo culture, CultureInfo uiCulture) => CultureChanger.ChangeCulture(culture, uiCulture);

		/// <summary>
		/// Устанавливает новые культуры приложения.
		/// </summary>
		/// <param name="culture">Объект, представляющий язык и региональные параметры, используемые текущим приложением. Это же значение используется для пользовательского интерфейса.</param>
		protected virtual void ChangeCulture(CultureInfo culture) => CultureChanger.ChangeCulture(culture, culture);

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					// Здесь необходимо освободить управляемое состояние (управляемые объекты).

					CultureChanger.CultureChanged -= CultureChanged;
				}

				// Здесь необходимо освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить метод завершения.
				// Здесь необходимо установить значение NULL для больших полей.
				_disposedValue = true;
			}
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			// Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}

using System.Globalization;

using Localization.Infrastructure;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Application.Components;

/// <summary>
/// Компонент для переключения культуры приложения. Для используемых в приложении двух культур выполняет циклическое переключение между ними.
/// </summary>
public sealed partial class CultureSwitcher
{
	/// <summary>
	/// Локализатор строк.
	/// </summary>
	[Inject] private IStringLocalizer<CultureSwitcher> Localizer { get; set; } = null!;

	/// <summary>
	/// Выбирает культуру из списка поддерживаемых культур, не равную текущей.
	/// </summary>
	private CultureInfo NewCulture => SupportedUICultures
		.First(item => item.LCID != CultureChanger.CurrentUICulture.LCID);

	/// <summary>
	/// Устанавливает новые культуры приложения.
	/// </summary>
	private void ChangeCulture() => ChangeCulture(NewCulture, NewCulture);
}
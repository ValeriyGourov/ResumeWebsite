#pragma warning disable CA1515

using System.Globalization;

using Application.Data.Models;

using Localization.Infrastructure;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Application.Components;

/// <summary>
/// Компонент для переключения культуры приложения. Для используемых в приложении двух культур
/// выполняет циклическое переключение между ними.
/// </summary>
public sealed partial class CultureSwitcher
{
	/// <summary>
	/// Заголовок страницы приложения.
	/// </summary>
	private string? _title;

	/// <summary>
	/// Данные для отображения в резюме.
	/// </summary>
	[Inject] private ResumeData ResumeData { get; set; } = null!;

	/// <summary>
	/// Локализатор строк.
	/// </summary>
	[Inject] private IStringLocalizer<CultureSwitcher> Localizer { get; set; } = null!;

	/// <summary>
	/// Выбирает культуру из списка поддерживаемых культур, не равную текущей.
	/// </summary>
	private CultureInfo NewCulture => SupportedUICultures
		.First(item => item.LCID != CultureChanger.CurrentUICulture.LCID);

	/// <inheritdoc/>
	protected override Task OnInitializedAsync()
	{
		SetTitle();
		return base.OnInitializedAsync();
	}

	/// <summary>
	/// Устанавливает новые культуры приложения.
	/// </summary>
	private void ChangeCulture()
	{
		ChangeCulture(NewCulture, NewCulture);
		SetTitle();
	}

	/// <summary>
	/// Устанавливает заголовок страницы приложения.
	/// </summary>
	private void SetTitle()
	{
		string? titleTemplate = Localizer["SiteTitle"];
		if (titleTemplate is not null)
		{
			_title = string.Format(
				CultureInfo.CurrentCulture,
				titleTemplate,
				ResumeData.Name,
				ResumeData.Surname);
		}
	}
}

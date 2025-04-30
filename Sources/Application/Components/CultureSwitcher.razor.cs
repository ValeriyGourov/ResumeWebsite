#pragma warning disable CA1515

using System.Globalization;

using Application.Data.Models;

using Localization.Components;
using Localization.Infrastructure;

using Microsoft.Extensions.Localization;

namespace Application.Components;

/// <summary>
/// Компонент для переключения культуры приложения. Для используемых в приложении двух культур
/// выполняет циклическое переключение между ними.
/// </summary>
/// <param name="logger">
/// <inheritdoc
///		cref="CultureSwitcherBase(ILogger{CultureSwitcherBase})"
///		path="/param[@name='logger']"/>
/// </param>
/// <param name="resumeData">Данные для отображения в резюме.</param>
/// <param name="localizer">Локализатор строк.</param>
public sealed partial class CultureSwitcher(
	ILogger<CultureSwitcher> logger,
	ResumeData resumeData,
	IStringLocalizer<CultureSwitcher> localizer)
	: CultureSwitcherBase(logger)
{
	/// <summary>
	/// Заголовок страницы приложения.
	/// </summary>
	private string? _title;

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
		_title = string.Format(
			CultureInfo.CurrentCulture,
			localizer["SiteTitle"],
			resumeData.Name,
			resumeData.Surname);
	}
}

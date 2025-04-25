using System.Globalization;

using Application.Data.Models;
using Application.Infrastructure.JavaScriptModules.Shared;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

using Toolbelt.Blazor.HeadElement;

namespace Application.Shared;

/// <summary>
/// Основная разметка страниц приложения.
/// </summary>
public partial class MainLayout : IAsyncDisposable
{
	[Inject] private MainLayoutJavaScriptModule JSModule { get; set; } = null!;

	/// <summary>
	/// Данные для отображения в резюме.
	/// </summary>
	[Inject] private ResumeData ResumeData { get; set; } = null!;

	/// <summary>
	/// Локализатор строк компонента.
	/// </summary>
	[Inject] private IStringLocalizer<MainLayout> Localizer { get; set; } = null!;

	/// <summary>
	/// Инструмент для изменения данных заголовка страницы.
	/// </summary>
	[Inject] private IHeadElementHelper HeadElementHelper { get; set; } = null!;

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await JSModule.ShowMainContainer().ConfigureAwait(true);
		}

		string? titleTemplate = Localizer["SiteTitle"];
		if (titleTemplate is not null)
		{
			string title = string.Format(
				CultureInfo.CurrentCulture,
				titleTemplate,
				ResumeData.Name,
				ResumeData.Surname);
			await HeadElementHelper.SetTitleAsync(title);
		}

		await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
	}

	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore().ConfigureAwait(false);
		GC.SuppressFinalize(this);
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		if (JSModule is not null)
		{
			await JSModule
				.DisposeAsync()
				.ConfigureAwait(false);
		}

		JSModule = null!;
	}
}

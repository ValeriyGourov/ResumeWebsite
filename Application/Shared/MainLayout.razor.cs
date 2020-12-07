using System.Globalization;
using System.Threading.Tasks;

using Application.Data.Models;
using Application.Infrastructure;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

using Toolbelt.Blazor.HeadElement;

namespace Application.Shared
{
	public partial class MainLayout
	{
		/// <summary>
		/// Обёртка для вызова функций JavaScript.
		/// </summary>
		[Inject] private MainJavaScriptWrapper JSWrapper { get; set; } = null!;

		[Inject] private ResumeData ResumeData { get; set; } = null!;

		[Inject] private IStringLocalizer<MainLayout> Localizer { get; set; } = null!;

		[Inject] private IHeadElementHelper HeadElementHelper { get; set; } = null!;

		//private string Title =>
		//	string.Format(
		//		CultureInfo.CurrentCulture,
		//		Localizer["SiteTitle"],
		//		ResumeData.Name,
		//		ResumeData.Surname);

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await JSWrapper.ShowMainContainer().ConfigureAwait(true);
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
	}
}

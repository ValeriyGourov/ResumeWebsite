using System.Globalization;
using System.Threading.Tasks;

using Application.Data.Models;
using Application.Infrastructure;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

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

		private string Title =>
			string.Format(
				CultureInfo.CurrentCulture,
				Localizer["SiteTitle"],
				ResumeData.Name,
				ResumeData.Surname);

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await JSWrapper.ShowMainContainer();
			}

			await base.OnAfterRenderAsync(firstRender);
		}
	}
}

using System.Threading.Tasks;

using Application.Infrastructure;

using Microsoft.AspNetCore.Components;

namespace Application.Shared
{
	public partial class MainLayout
	{
		/// <summary>
		/// Обёртка для вызова функций JavaScript.
		/// </summary>
		[Inject] private MainJavaScriptWrapper JSWrapper { get; set; } = null!;

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

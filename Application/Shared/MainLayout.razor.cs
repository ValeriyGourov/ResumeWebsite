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

		protected override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				JSWrapper.ShowMainContainer();
			}

			return base.OnAfterRenderAsync(firstRender);
		}
	}
}

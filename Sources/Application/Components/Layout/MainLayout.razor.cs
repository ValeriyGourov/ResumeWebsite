#pragma warning disable CA1515

using Application.Infrastructure.JavaScriptModules.Shared;

using Microsoft.AspNetCore.Components;

namespace Application.Components.Layout;

/// <summary>
/// Основная разметка страниц приложения.
/// </summary>
public sealed partial class MainLayout : IAsyncDisposable
{
	[Inject] private MainLayoutJavaScriptModule JSModule { get; set; } = null!;

	/// <inheritdoc/>
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await JSModule.ShowMainContainer().ConfigureAwait(true);
		}

		await base.OnAfterRenderAsync(firstRender).ConfigureAwait(true);
	}

	/// <inheritdoc/>
	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore().ConfigureAwait(false);
		GC.SuppressFinalize(this);
	}

	/// <inheritdoc/>
	private async ValueTask DisposeAsyncCore()
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

#pragma warning disable CA1515

using System.Globalization;

using Application.Data.Models;
using Application.Infrastructure.JavaScriptModules.Shared;
using Application.Services.PdfGeneration;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

using SoloX.BlazorJsBlob;

namespace Application.Components.Layout;

/// <summary>
/// Основная разметка страниц приложения.
/// </summary>
public sealed partial class MainLayout(
	IStringLocalizer<MainLayout> localizer,
	IBlobService blobService,
	IPdfGenerator pdfGenerator,
	ResumeData resumeData)
	: IAsyncDisposable
{
	private readonly IBlobService _blobService = blobService;
	private readonly IPdfGenerator _pdfGenerator = pdfGenerator;
	private readonly ResumeData _resumeData = resumeData;

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
		await _blobService.DisposeAsync().ConfigureAwait(false);

		if (JSModule is not null)
		{
			await JSModule
				.DisposeAsync()
				.ConfigureAwait(false);
		}

		JSModule = null!;
	}

	/// <summary>
	/// Формирование и загрузка данных резюме в формате PDF.
	/// </summary>
	private async Task DownloadPdfAsync()
	{
		Stream docStream = await _pdfGenerator
			.GenerateAsync()
			.ConfigureAwait(false);
		await using (docStream)
		{
			IBlob blob = await _blobService
				.CreateBlobAsync(docStream)
				.ConfigureAwait(false);
			await using (blob)
			{
				string fileName = string.Format(
					CultureInfo.CurrentCulture,
					localizer["ResumeFileName"],
					_resumeData.Name,
					_resumeData.Surname);

				await _blobService
					.SaveAsFileAsync(blob, fileName)
					.ConfigureAwait(false);
			}
		}
	}
}

using System.Diagnostics.CodeAnalysis;

using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Application.Services.PdfGeneration;

/// <summary>
/// Предоставляет функциональность для создания PDF-документов с помощью библиотеки QuestPDF.
/// </summary>
/// <remarks>
/// Этот класс отвечает за создание PDF-документов на основе предоставленного шаблона
/// документа. Он поддерживает генерацию PDF в виде потока и, при необходимости, отображение
/// документа в приложении QuestPDF Companion, если включён соответствующий параметр
/// конфигурации.
/// </remarks>
/// <param name="logger">Средство ведения журнала.</param>
/// <param name="configuration">Конфигурация приложения.</param>
/// <param name="document">Экземпляр класса документа, формирующий файл PDF.</param>
internal sealed partial class PdfGenerator(
	ILogger<PdfGenerator> logger,
	IConfiguration configuration,
	IDocument document)
	: IPdfGenerator
{
	private readonly IConfiguration _configuration = configuration;
	private readonly IDocument _document = document;

	static PdfGenerator()
	{
		QuestPDF.Settings.License = LicenseType.Community;
		QuestPDF.Settings.UseEnvironmentFonts = false;
	}

	public async ValueTask<Stream> GenerateAsync()
	{
		if (_configuration.GetValue("PdfGeneration:ShowInCompanion", false))
		{
			LogInformationPdfToCompanion();

			await _document
				.ShowInCompanionAsync()
				.ConfigureAwait(false);
		}

		MemoryStream memoryStream = new();

		_document.GeneratePdf(memoryStream);
		memoryStream.Position = 0;

		return memoryStream;
	}

	#region Методы журналирования

	[ExcludeFromCodeCoverage]
	[LoggerMessage(
		Level = LogLevel.Information,
		Message = @"PDF будет сформирован в запущенном приложении ""QuestPDF Companion"".")]
	private partial void LogInformationPdfToCompanion();

	#endregion
}

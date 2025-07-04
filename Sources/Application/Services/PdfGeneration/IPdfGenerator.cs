#pragma warning disable CA1515

namespace Application.Services.PdfGeneration;

/// <summary>
/// Определяет контракт для асинхронной генерации PDF-документов.
/// </summary>
/// <remarks>
/// Реализации этого интерфейса отвечают за создание PDF-документов и возврат их в виде потока.
/// Вызывающая сторона отвечает за утилизацию возвращённого потока после его использования.
/// </remarks>
public interface IPdfGenerator
{
	/// <summary>
	/// Асинхронно генерирует поток, содержащий выходные данные.
	/// </summary>
	/// <remarks>
	/// Вызывающая сторона отвечает за утилизацию возвращаемого <see cref="Stream"/>,
	/// чтобы освободить все связанные с ним ресурсы.
	/// </remarks>
	/// <returns>
	/// <see cref="ValueTask{TResult}"/>, который представляет асинхронную операцию.
	/// Результат содержит <see cref="Stream"/> со сгенерированными выходными данными документа.
	/// </returns>
	ValueTask<Stream> GenerateAsync();
}

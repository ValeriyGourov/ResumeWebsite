using System.ComponentModel.DataAnnotations;

namespace Application.Infrastructure.Validation;

/// <summary>
/// Представляет контейнер для результатов запроса на рекурсивную проверку сложных типов.
/// </summary>
/// <param name="errorMessage">Сообщение об ошибке.</param>
/// <param name="validationResults">
/// Список результатов запросов на проверку вложенных объектов.
/// </param>
internal sealed class ComplexTypeValidationResult(
	string errorMessage,
	IEnumerable<ValidationResult> validationResults)
	: ValidationResult(errorMessage)
{
	/// <summary>
	/// Результаты запросов на проверку вложенных объектов.
	/// </summary>
	public IEnumerable<ValidationResult> ValidationResults { get; } = validationResults
		?? throw new ArgumentNullException(nameof(validationResults));
}

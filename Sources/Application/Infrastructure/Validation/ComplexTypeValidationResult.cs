using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Infrastructure.Validation;

/// <summary>
/// Представляет контейнер для результатов запроса на рекурсивную проверку сложных типов.
/// </summary>
public class ComplexTypeValidationResult : ValidationResult
{
	/// <summary>
	/// Результаты запросов на проверку вложенных объектов.
	/// </summary>
	public IEnumerable<ValidationResult> ValidationResults { get; }

	/// <summary>
	/// Инициализирует новый экземпляр класса <see cref="ComplexTypeValidationResult"/> с использованием указанного сообщения об ошибке и списка результатов запросов на проверку вложенных объектов.
	/// </summary>
	/// <param name="errorMessage">Сообщение об ошибке.</param>
	/// <param name="validationResults">Список результатов запросов на проверку вложенных объектов.</param>
	public ComplexTypeValidationResult(string errorMessage, IEnumerable<ValidationResult> validationResults)
		: base(errorMessage)
	{
		ValidationResults = validationResults ?? throw new ArgumentNullException(nameof(validationResults));
	}
}
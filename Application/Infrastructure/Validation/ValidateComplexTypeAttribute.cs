using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Infrastructure.Validation
{
	/// <summary>
	/// Атрибут рекурсивной проверки сложных типов.
	/// </summary>
	public class ValidateComplexTypeAttribute : ValidationAttribute
	{
		/// <inheritdoc/>
		/// <remarks>При неуспешной проверке возвращает экземпляр класса <see cref="ComplexTypeValidationResult"/>, включающего результаты рекурсивной проверки свойств проверяемого объекта.</remarks>
		/// <exception cref="ArgumentNullException">Не указан контекст проверки.</exception>
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value is null)
			{
				return ValidationResult.Success;
			}

			if (validationContext is null)
			{
				throw new ArgumentNullException(nameof(validationContext));
			}

			List<ValidationResult> validationResults = new List<ValidationResult>();
			if (value is IEnumerable collection)
			{
				int index = -1;

				foreach (object? item in collection)
				{
					index++;

					if (item is null)
					{
						continue;
					}

					List<ValidationResult> itemValidationResults = new List<ValidationResult>();
					ValidationContext itemValidationContext = ValidateValue(item, itemValidationResults);

					if (itemValidationResults.Count > 0)
					{
						ComplexTypeValidationResult itemValidationResult = new ComplexTypeValidationResult(
							FormatErrorMessage($"{itemValidationContext.DisplayName}[{index}]"),
							itemValidationResults);

						validationResults.Add(itemValidationResult);
					}
				}
			}
			else
			{
				ValidateValue(value, validationResults);
			}

			return validationResults.Count == 0
				? ValidationResult.Success
				: new ComplexTypeValidationResult(
					FormatErrorMessage(validationContext.DisplayName),
					validationResults);
		}

		/// <summary>
		/// Выполняет проверку отдельно взятого объекта.
		/// </summary>
		/// <param name="value">Значение для проверки.</param>
		/// <param name="validationResults">Коллекция для хранения всех проверок, завершившихся неудачей.</param>
		/// <returns>Контекст, описывающий проверяемый объект.</returns>
		private static ValidationContext ValidateValue(object value, List<ValidationResult> validationResults)
		{
			ValidationContext validationContext = new ValidationContext(value);
			Validator.TryValidateObject(value, validationContext, validationResults, true);

			return validationContext;
		}
	}
}

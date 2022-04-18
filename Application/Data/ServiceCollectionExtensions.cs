using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

using Application.Data.Models;
using Application.Infrastructure.Validation;

namespace Application.Data
{
	/// <summary>
	/// Расширения <see cref="IServiceCollection"/>, подключающих данные резюме.
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Относительный путь к файлу с данными резюме.
		/// </summary>
		private const string _resumeDataPath = "Data/ResumeData.json";

		/// <summary>
		/// Добавляет объект с данными резюме в коллекцию служб.
		/// </summary>
		/// <param name="services">Коллекция служб.</param>
		/// <returns>Коллекция служб, которая может быть использована для дальнейшей настройки конфигурации.</returns>
		public static IServiceCollection AddResumeData(this IServiceCollection services)
		{
			string json = File.ReadAllText(_resumeDataPath);
			JsonSerializerOptions serializerOptions = new JsonSerializerOptions
			{
				IgnoreNullValues = true,
				ReadCommentHandling = JsonCommentHandling.Skip
			};

			ResumeData? resumeData = JsonSerializer.Deserialize<ResumeData>(json, serializerOptions);
			if (resumeData is null)
			{
				const string errorMessage = "Не удалось прочитать данные резюме.";
				throw new ApplicationException(errorMessage);
			}

			List<ValidationResult> validationResults = new List<ValidationResult>();
			ValidationContext validationContext = new ValidationContext(resumeData);

			if (Validator.TryValidateObject(resumeData, validationContext, validationResults, true))
			{
				services.AddSingleton(resumeData);
			}
			else
			{
				const string errorMessage = "В файле данных указаны некорректные данные.";

				StringBuilder stringBuilder = new StringBuilder(errorMessage);
				stringBuilder.AppendLine();
				BuildErrorDetails(stringBuilder, validationResults);

				throw new ApplicationException(stringBuilder.ToString());
			}

			return services;
		}

		/// <summary>
		/// Формирует строку с иерархическим перечислением ошибок проверки данных резюме.
		/// </summary>
		/// <param name="stringBuilder">Построитель текста строки с ошибками.</param>
		/// <param name="validationResults">Коллекция результатов проверки данных резюме.</param>
		/// <param name="indentLevel">Уровень текущего вызова метода в иерархии описания ошибок. На указанное значение будет выполнен отступ добавляемого текста.</param>
		private static void BuildErrorDetails(
			StringBuilder stringBuilder,
			IEnumerable<ValidationResult> validationResults,
			int indentLevel = 0)
		{
			foreach (ValidationResult? error in validationResults)
			{
				stringBuilder
					.Append('\t', indentLevel)
					.AppendLine(error.ErrorMessage);

				if (error is ComplexTypeValidationResult complexTypeResult)
				{
					BuildErrorDetails(
						stringBuilder,
						complexTypeResult.ValidationResults,
						indentLevel + 1);
				}
			}
		}
	}
}
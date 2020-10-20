using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Text.Json;

using Application.Data.Models;
using Application.Infrastructure.Validation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
		/// <typeparam name="T">Тип стартового класса, из которого выполняется вызов метода.</typeparam>
		/// <param name="services">Коллекция служб.</param>
		/// <returns>Коллекция служб, которая может быть использована для дальнейшей настройки конфигурации.</returns>
		public static IServiceCollection AddResumeData<T>(this IServiceCollection services)
		{
			string json = File.ReadAllText(_resumeDataPath);
			JsonSerializerOptions serializerOptions = new JsonSerializerOptions
			{
				IgnoreNullValues = true,
				ReadCommentHandling = JsonCommentHandling.Skip
			};
			ResumeData resumeData = JsonSerializer.Deserialize<ResumeData>(json, serializerOptions);

			List<ValidationResult> validationResults = new List<ValidationResult>();
			ValidationContext validationContext = new ValidationContext(resumeData);

			if (Validator.TryValidateObject(resumeData, validationContext, validationResults, true))
			{
				services.AddSingleton(resumeData);
			}
			else
			{
				ILogger logger = services
					.BuildServiceProvider()
					.GetRequiredService<ILogger<T>>();

				const string errorMessage = "В файле данных указаны некорректные данные.";

				StringBuilder stringBuilder = new StringBuilder(errorMessage);
				stringBuilder.AppendLine();
				BuildErrorDetails(stringBuilder, validationResults);
				logger.LogCritical(stringBuilder.ToString());

				throw new ApplicationException(errorMessage);
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

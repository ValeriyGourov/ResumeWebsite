using System.Text.Json;
using System.Text.Json.Serialization;

using Application.Data.Models;

using CommunityToolkit.Diagnostics;

using FluentValidation;

namespace Application.Data;

/// <summary>
/// Расширения <see cref="IServiceCollection"/>, подключающих данные резюме.
/// </summary>
internal static class ServiceCollectionExtensions
{
	/// <summary>
	/// Относительный путь к файлу с данными резюме.
	/// </summary>
	private const string _resumeDataPath = "Data/ResumeData.json";

	private static readonly JsonSerializerOptions _serializerOptions = new()
	{
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		ReadCommentHandling = JsonCommentHandling.Skip
	};

	/// <summary>
	/// Добавляет объект с данными резюме в коллекцию служб.
	/// </summary>
	/// <param name="services">Коллекция служб.</param>
	/// <returns>
	/// Коллекция служб, которая может быть использована для дальнейшей настройки конфигурации.
	/// </returns>
	public static IServiceCollection AddResumeData(this IServiceCollection services)
	{
		Guard.IsNotNull(services);

		string json = File.ReadAllText(_resumeDataPath);

		ResumeData resumeData = JsonSerializer.Deserialize<ResumeData>(json, _serializerOptions)
			?? throw new InvalidOperationException("Не удалось прочитать данные резюме.");

		new ResumeDataValidator().ValidateAndThrow(resumeData);
		_ = services.AddSingleton(resumeData);

		return services;
	}
}

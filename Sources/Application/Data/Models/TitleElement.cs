#pragma warning disable CA1515

using System.ComponentModel.DataAnnotations;

using Application.Infrastructure.Validation;

namespace Application.Data.Models;

/// <summary>
/// Элемент данных, содержащий название и описание.
/// </summary>
/// <param name="Title">Название.</param>
/// <param name="Description">Детальное описание.</param>
public record TitleElement(
	[property: Required, ValidateComplexType] DataString Title,
	[property: Required, ValidateComplexType] DataString Description);

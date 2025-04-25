using System.ComponentModel.DataAnnotations;

using Application.Infrastructure.Validation;

namespace Application.Data.Models;

/// <summary>
/// Описание навыка, измеренного в процентах.
/// </summary>
/// <param name="Title">Название навыка.</param>
/// <param name="Percent">Степень владения навыком, выраженная в процентах от 0 до 100.</param>
public sealed record SkillItem(
	[property: Required, ValidateComplexType] DataString Title,
	[property: Range(1, 100)] byte Percent);
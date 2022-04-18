using System.ComponentModel.DataAnnotations;

using Application.Infrastructure.Validation;

namespace Application.Data.Models;

/// <summary>
/// Элемент данных, содержащий название и описание.
/// </summary>
public class TitleElement
{
	/// <summary>
	/// Название.
	/// </summary>
	[Required, ValidateComplexType]
	public DataString Title { get; set; } = null!;

	/// <summary>
	/// Детальное описание.
	/// </summary>
	[Required, ValidateComplexType]
	public DataString Description { get; set; } = null!;
}
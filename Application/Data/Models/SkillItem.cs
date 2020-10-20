using System.ComponentModel.DataAnnotations;

using Application.Infrastructure.Validation;

namespace Application.Data.Models
{
	/// <summary>
	/// Описание навыка, измеренного в процентах.
	/// </summary>
	public sealed class SkillItem
	{
		/// <summary>
		/// Название навыка.
		/// </summary>
		[Required, ValidateComplexType]
		public DataString Title { get; set; } = null!;

		/// <summary>
		/// Формальное значение навыка, выраженное в процентах.
		/// </summary>
		[Range(1, 100)]
		public byte Percent { get; set; }
	}
}

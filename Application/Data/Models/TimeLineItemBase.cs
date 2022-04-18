using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using Application.Infrastructure.Validation;

namespace Application.Data.Models;

/// <summary>
/// Базовый класс для событий на временной линии. В наследованных классах должен быть реализован метод для сравнения/сортировки событий.
/// </summary>
public abstract class TimeLineItemBase : IComparable<TimeLineItemBase>
{
	/// <summary>
	/// Организация или учреждение, в котором происходило событие.
	/// </summary>
	[Required, ValidateComplexType]
	public DataString Institution { get; set; } = null!;

	/// <summary>
	/// Занимаемая позиция в организации или учреждении.
	/// </summary>
	[Required, ValidateComplexType]
	public DataString Position { get; set; } = null!;

	/// <summary>
	/// Местоположение организации или учреждении.
	/// </summary>
	[Required, ValidateComplexType]
	public DataString Location { get; set; } = null!;

	/// <summary>
	/// Описание деятельности в организации или учреждении.
	/// </summary>
	[Required, ValidateComplexType]
	public DataString Description { get; set; } = null!;

	public abstract int CompareTo([AllowNull] TimeLineItemBase other);

	public override bool Equals(object? obj) => base.Equals(obj);

	public override int GetHashCode() => base.GetHashCode();

	public static bool operator ==(TimeLineItemBase? left, TimeLineItemBase? right)
	{
		if (left is null)
		{
			return right is null;
		}

		return left.Equals(right);
	}

	public static bool operator !=(TimeLineItemBase? left, TimeLineItemBase? right) => !(left == right);

	public static bool operator <(TimeLineItemBase? left, TimeLineItemBase? right) =>
		left is null
			? right is not null
			: left.CompareTo(right) < 0;

	public static bool operator <=(TimeLineItemBase? left, TimeLineItemBase? right) =>
		left is null || left.CompareTo(right) <= 0;

	public static bool operator >(TimeLineItemBase? left, TimeLineItemBase? right) =>
		left?.CompareTo(right) > 0;

	public static bool operator >=(TimeLineItemBase? left, TimeLineItemBase? right) =>
		left is null
			? right is null
			: left.CompareTo(right) >= 0;
}
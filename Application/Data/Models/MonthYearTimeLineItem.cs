using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.Data.Models;

/// <summary>
/// Событие на временной линии с периодом в виде месяца и года.
/// </summary>
public sealed class MonthYearTimeLineItem : TimeLineItemBase
{
	private DateTimeOffset _startDate;
	private DateTimeOffset? _endDate;

	/// <summary>
	/// Дата начала события. При присваивании значения дата преобразуется к началу месяца.
	/// </summary>
	[Required]
	public DateTimeOffset StartDate
	{
		get => _startDate;
		set => _startDate = MonthBeginning(value);
	}

	/// <summary>
	/// Дата окончания события. Если не указана, то событие считается активным. При присваивании значения дата преобразуется к началу месяца.
	/// </summary>
	public DateTimeOffset? EndDate
	{
		get => _endDate;
		set => _endDate = value.HasValue ? MonthBeginning(value.Value) : value;
	}

	public override int CompareTo([AllowNull] TimeLineItemBase other)
	{
		if (other is not MonthYearTimeLineItem typedOther)
		{
			return 1;
		}

		DateTimeOffset utcNow = DateTimeOffset.UtcNow;
		DateTimeOffset otherEndDate = typedOther.EndDate ?? utcNow;
		DateTimeOffset endDate = EndDate ?? utcNow;

		int compareResult = otherEndDate.CompareTo(endDate);
		if (compareResult == 0)
		{
			compareResult = typedOther.StartDate.CompareTo(StartDate);
		}

		return compareResult;
	}

	/// <summary>
	/// Преобразует дату к началу месяца.
	/// </summary>
	/// <param name="dateTime">Исходная дата.</param>
	/// <returns>Дата первого числа месяца.</returns>
	private static DateTimeOffset MonthBeginning(DateTimeOffset dateTime) => new(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, TimeSpan.Zero);
}
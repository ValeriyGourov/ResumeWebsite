using System.Diagnostics.CodeAnalysis;

namespace Application.Data.Models;

/// <summary>
/// Событие на временной линии с периодом в виде месяца и года.
/// </summary>
/// <param name="Institution"><inheritdoc cref="TimeLineItemBase" path="/param[@name='Institution']"/></param>
/// <param name="Position"><inheritdoc cref="TimeLineItemBase" path="/param[@name='Position']"/></param>
/// <param name="Location"><inheritdoc cref="TimeLineItemBase" path="/param[@name='Location']"/></param>
/// <param name="Description"><inheritdoc cref="TimeLineItemBase" path="/param[@name='Description']"/></param>
/// <param name="StartDate"><inheritdoc cref="StartDate" path="/summary"/></param>
/// <param name="EndDate"><inheritdoc cref="EndDate" path="/summary"/></param>
public sealed record MonthYearTimeLineItem(
	DataString Institution,
	DataString Position,
	DataString Location,
	DataString Description,
	DateTimeOffset StartDate,
	DateTimeOffset? EndDate)
	: TimeLineItemBase(Institution, Position, Location, Description)
{
	/// <summary>
	/// Дата начала события. При присваивании значения дата преобразуется к началу месяца.
	/// </summary>
	public DateTimeOffset StartDate { get; init; } = MonthBeginning(StartDate);

	/// <summary>
	/// Дата окончания события. Если не указана, то событие считается активным. При присваивании значения дата преобразуется к началу месяца.
	/// </summary>
	public DateTimeOffset? EndDate { get; init; } = EndDate.HasValue ? MonthBeginning(EndDate.Value) : EndDate;

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
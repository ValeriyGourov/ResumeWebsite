#pragma warning disable CA1515

using System.Diagnostics.CodeAnalysis;

namespace Application.Data.Models;

/// <summary>
/// Событие на временной линии с периодом в виде месяца и года.
/// </summary>
/// <param name="Institution">
/// <inheritdoc cref="TimeLineItemBase{T}" path="/param[@name='Institution']"/>
/// </param>
/// <param name="Position">
/// <inheritdoc cref="TimeLineItemBase{T}" path="/param[@name='Position']"/>
/// </param>
/// <param name="Location">
/// <inheritdoc cref="TimeLineItemBase{T}" path="/param[@name='Location']"/>
/// </param>
/// <param name="Description">
/// <inheritdoc cref="TimeLineItemBase{T}" path="/param[@name='Description']"/>
/// </param>
/// <param name="StartDate"><inheritdoc cref="StartDate" path="/summary"/></param>
/// <param name="EndDate"><inheritdoc cref="EndDate" path="/summary"/></param>
public sealed record MonthYearTimeLineItem(
	DataString Institution,
	DataString Position,
	DataString Location,
	DataString Description,
	DateOnly StartDate,
	DateOnly? EndDate)
	: TimeLineItemBase<MonthYearTimeLineItem>(Institution, Position, Location, Description)
{
	/// <summary>
	/// Дата начала события. При присваивании значения дата преобразуется к началу месяца.
	/// </summary>
	public DateOnly StartDate { get; init; } = MonthBeginning(StartDate);

	/// <summary>
	/// Дата окончания события. Если не указана, то событие считается активным.
	/// При присваивании значения дата преобразуется к началу месяца.
	/// </summary>
	public DateOnly? EndDate { get; init; } = EndDate.HasValue
		? MonthBeginning(EndDate.Value)
		: null;

	/// <inheritdoc/>
	public override int CompareTo(MonthYearTimeLineItem? other)
	{
		if (other is null)
		{
			return 1;
		}

		DateOnly now = DateOnly.FromDateTime(DateTime.UtcNow);
		DateOnly otherEndDate = other.EndDate ?? now;
		DateOnly endDate = EndDate ?? now;

		int compareResult = otherEndDate.CompareTo(endDate);
		if (compareResult == 0)
		{
			compareResult = other.StartDate.CompareTo(StartDate);
		}

		return compareResult;
	}

	/// <summary>
	/// Преобразует дату к началу месяца.
	/// </summary>
	/// <param name="date">Исходная дата.</param>
	/// <returns>Дата первого числа месяца.</returns>
	private static DateOnly MonthBeginning(DateOnly date)
		=> new(date.Year, date.Month, 1);
}

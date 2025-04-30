#pragma warning disable CA1515

using System.Diagnostics.CodeAnalysis;

namespace Application.Data.Models;

/// <summary>
/// Событие на временной линии с периодом в виде года.
/// </summary>
/// <param name="Institution"><inheritdoc cref="TimeLineItemBase" path="/param[@name='Institution']"/></param>
/// <param name="Position"><inheritdoc cref="TimeLineItemBase" path="/param[@name='Position']"/></param>
/// <param name="Location"><inheritdoc cref="TimeLineItemBase" path="/param[@name='Location']"/></param>
/// <param name="Description"><inheritdoc cref="TimeLineItemBase" path="/param[@name='Description']"/></param>
/// <param name="StartYear">Год начала события.</param>
/// <param name="EndYear">Год окончания события. Если не указан, то событие считается активным.</param>
public sealed record YearTimeLineItem(
	DataString Institution,
	DataString Position,
	DataString Location,
	DataString Description,
	int StartYear,
	int? EndYear)
	: TimeLineItemBase(Institution, Position, Location, Description)
{
	/// <inheritdoc/>
	public override int CompareTo([AllowNull] TimeLineItemBase other)
	{
		if (other is not YearTimeLineItem typedOther)
		{
			return 1;
		}

		int yearNow = DateTimeOffset.UtcNow.Year;
		int otherEndYear = typedOther.EndYear ?? yearNow;
		int endYear = EndYear ?? yearNow;

		int compareResult = otherEndYear.CompareTo(endYear);
		if (compareResult == 0)
		{
			compareResult = typedOther.StartYear.CompareTo(StartYear);
		}

		return compareResult;
	}
}

using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.Data.Models;

/// <summary>
/// Событие на временной линии с периодом в виде года.
/// </summary>
public sealed class YearTimeLineItem : TimeLineItemBase
{
	/// <summary>
	/// Год начала события.
	/// </summary>
	[Required]
	public int StartYear { get; set; }

	/// <summary>
	/// Год окончания события. Если не указан, то событие считается активным.
	/// </summary>
	public int? EndYear { get; set; }

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
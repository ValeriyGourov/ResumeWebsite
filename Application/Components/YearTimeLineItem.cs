using System.Globalization;

using Microsoft.AspNetCore.Components;

namespace Application.Components
{
	public class YearTimeLineItem : TimeLineItemBase
	{
		/// <summary>
		/// Год начала события.
		/// </summary>
		[Parameter]
		public int StartYear { get; set; }

		/// <summary>
		/// Год окончания события. Если не указан, то событие считается активным.
		/// </summary>
		[Parameter]
		public int? EndYear { get; set; }

		protected override void OnParametersSet()
		{
			StartPeriod = StartYear.ToString(CultureInfo.CurrentCulture);
			EndPeriod = EndYear?.ToString(CultureInfo.CurrentCulture);
		}
	}
}

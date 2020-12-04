using System;

using Microsoft.AspNetCore.Components;

namespace Application.Components
{
	/// <summary>
	/// Навык, измеренный в процентах.
	/// </summary>
	public partial class Skill
	{
		private byte _percent;

		/// <summary>
		/// Степень владения навыком, выраженная в процентах от 0 до 100.
		/// </summary>
		[Parameter]
		public byte Percent
		{
			get => _percent;
			set
			{
				if (value > 100)
				{
					throw new ArgumentOutOfRangeException(nameof(value), value, "Значение процентов должно быть в диапазоне от 0 до 100.");
				}
				_percent = value;
			}
		}

		/// <summary>
		/// Название навыка.
		/// </summary>
		[Parameter]
		public string Title { get; set; } = null!;
	}
}

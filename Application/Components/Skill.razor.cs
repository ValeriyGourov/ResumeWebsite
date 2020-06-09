using System;

using Microsoft.AspNetCore.Components;

namespace Application.Components
{
	public partial class Skill
	{
		private byte _percent;

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

		[Parameter]
		public string Title { get; set; } = null!;
	}
}

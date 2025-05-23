#pragma warning disable CA1515

using Microsoft.AspNetCore.Components;

namespace Application.Components;

/// <summary>
/// Навык, измеренный в процентах.
/// </summary>
public sealed partial class Skill
{
	/// <summary>
	/// Степень владения навыком, выраженная в процентах от 0 до 100.
	/// </summary>
	[Parameter]
	public byte Percent { get; set; }

	/// <summary>
	/// Название навыка.
	/// </summary>
	[Parameter]
	public string Title { get; set; } = null!;
}

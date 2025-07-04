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
	[Parameter, EditorRequired]
	public required byte Percent { get; set; }

	/// <summary>
	/// Название навыка.
	/// </summary>
	[Parameter, EditorRequired]
	public required string Title { get; set; }
}

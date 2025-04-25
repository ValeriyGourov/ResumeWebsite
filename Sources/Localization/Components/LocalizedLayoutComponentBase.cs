using Microsoft.AspNetCore.Components;

namespace Localization.Components;

/// <summary>
/// Базовый компонент, который представляет макет, с поддержкой переключения культуры приложения на лету. Все компоненты с макетом, требующие такой локализации, должны наследоваться от данного класса.
/// </summary>
public abstract class LocalizedLayoutComponentBase : LocalizedComponentBase
{
	/// <summary>
	/// Содержимое, которое будет выведено внутри макета.
	/// </summary>
	[Parameter] public RenderFragment Body { get; set; } = null!;
}
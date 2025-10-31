using System.Text;

using Microsoft.Extensions.ObjectPool;

namespace Extensions.ObjectPool;

/// <summary>
/// Пул экземпляров <see cref="StringBuilder"/>.
/// </summary>
public class StringBuilderPool : DefaultObjectPool<StringBuilder>
{
	/// <summary>
	/// Создаёт экземпляр <see cref="StringBuilderPool"/>.
	/// </summary>
	public StringBuilderPool()
		: base(new StringBuilderPooledObjectPolicy())
	{ }

	/// <inheritdoc cref="StringBuilderPool()" />
	/// <param name="maximumRetained">
	/// <inheritdoc
	///		cref="DefaultObjectPool{T}.DefaultObjectPool(IPooledObjectPolicy{T}, int)"
	///		path="/param[@name='maximumRetained']"/>
	/// </param>
	/// <param name="initialCapacity">
	/// <inheritdoc
	///		cref="StringBuilderPooledObjectPolicy.InitialCapacity"
	///		path="/summary"/>
	/// </param>
	/// <param name="maximumRetainedCapacity">
	/// <inheritdoc
	///		cref="StringBuilderPooledObjectPolicy.MaximumRetainedCapacity"
	///		path="/summary"/>
	/// </param>
	public StringBuilderPool(int maximumRetained, int initialCapacity, int maximumRetainedCapacity)
		: base(
			new StringBuilderPooledObjectPolicy()
			{
				InitialCapacity = initialCapacity,
				MaximumRetainedCapacity = maximumRetainedCapacity
			},
			maximumRetained)
	{ }

	/// <summary>
	/// Возвращает общий экземпляр <see cref="StringBuilderPool"/>.
	/// </summary>
	public static StringBuilderPool Shared { get; } = new();

	/// <summary>
	/// Строит строку с помощью арендуемого <see cref="StringBuilder"/> и возвращает
	/// его обратно в пул.
	/// </summary>
	/// <param name="action">
	/// Метод формирования строки с помощью арендованного <see cref="StringBuilder"/>.
	/// </param>
	/// <returns>Сформированная строка.</returns>
	public static string BuildString(Action<StringBuilder> action)
	{
		ArgumentNullException.ThrowIfNull(action);

		using ObjectPoolLease<StringBuilder> poolLease = Shared.Lease();

		action(poolLease.Value);
		return poolLease.Value.ToString();
	}
}

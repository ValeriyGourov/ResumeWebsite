#pragma warning disable CA1034 // TODO: Удалить после исправления ошибки анализатора: https://github.com/dotnet/roslyn-analyzers/issues/7765

using Microsoft.Extensions.ObjectPool;

namespace Extensions.ObjectPool;

/// <summary>
/// Обёртка над арендуемым из <see cref="ObjectPool{T}"/> экземпляра <typeparamref name="T"/>
/// с его автоматическим возвратом в пул.
/// </summary>
/// <remarks>
/// Данное средство аренды реализует шаблон освобождения и предназначено именно для автоматического
/// использования возврата арендованного значения в пул при выходе из области видимости. Для этого
/// используется конструкция <c>using</c>. При необходимости ручного управления временем жизни
/// арендуемого значения, имеет смысл использовать штатные механизмы работы с пулом класса
/// <see cref="ObjectPool{T}"/>.
/// </remarks>
/// <typeparam name="T">Тип объектов для объединения в пул.</typeparam>
/// <example>
/// Пример использования.
/// <code>
///		using (var poolLease = pool.Lease())
///		{
///			// Использование арендованного значения.
///			var pooledValue = poolLease.Value;
///		}
/// </code>
/// </example>
public ref struct ObjectPoolLease<T>
	where T : class
{
	private readonly ObjectPool<T> _pool;
	private T? _value;

	/// <summary>
	/// Арендованное значение.
	/// </summary>
	/// <exception cref="ObjectDisposedException">Попытка доступа к освобождённому объекту.</exception>
	public T Value => _value ?? throw new ObjectDisposedException(nameof(Value));

	/// <summary>
	/// Создание экземпляра <see cref="ObjectPoolLease{T}"/>.
	/// </summary>
	/// <param name="pool">Пул объектов, в котором арендуется экземпляр <typeparamref name="T"/>.</param>
	public ObjectPoolLease(ObjectPool<T> pool)
	{
		ArgumentNullException.ThrowIfNull(pool);

		_pool = pool;
		_value = pool.Get();
	}

	/// <summary>
	/// Выполняет задачи, определенные приложением, связанные с освобождением, сбросом или перезагрузкой ресурсов.
	/// </summary>
	public void Dispose()
	{
		if (_value is not null)
		{
			_pool.Return(_value);
			_value = null;
		}
	}
}

/// <summary>
/// Методы расширения для <see cref="ObjectPool{T}"/>.
/// </summary>
public static class ObjectPoolExtensions
{
	extension<T>(ObjectPool<T> pool)
		where T : class
	{
		/// <summary>
		/// Создает обёртку над арендуемым из пула объектов экземпляром <typeparamref name="T"/>.
		/// </summary>
		/// <returns>Обёртка над арендованным значением.</returns>
		public ObjectPoolLease<T> Lease() => new(pool);
	}
}

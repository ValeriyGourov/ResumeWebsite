using Extensions.ObjectPool;

using FluentAssertions;

using Microsoft.Extensions.ObjectPool;

namespace Extensions.UnitTests.ObjectPool;

[TestClass]
internal class ObjectPoolLeaseTests
{
	[TestMethod(DisplayName = "При освобождении экземпляра арендатора его арендованное значение должно быть возвращено в пул")]
	public void Dispose1()
	{
		// Arrange.
		TestObjectPool pool = new();

		// Act.
		using (ObjectPoolLease<object> poolLease = pool.Lease())
		{
			_ = poolLease.Value;
		}

		// Assert.
		pool.IsReturned.Should().BeTrue();
	}

	[TestMethod(DisplayName = "В пределах области using экземпляр аренды должен вернуть значение")]
	public void Value1()
	{
		// Arrange.
		TestObjectPool pool = new();

		// Act.
		Action act = () =>
		{
			using ObjectPoolLease<object> poolLease = pool.Lease();
			_ = poolLease.Value;
		};

		// Assert.
		act.Should().NotThrow<ObjectDisposedException>();
	}

	[TestMethod(DisplayName = "После освобождения экземпляра аренды получение его значения должно выбросить исключение")]
	public void Value2()
	{
		// Arrange.
		TestObjectPool pool = new();

		// Act.
		Action act = () =>
		{
			ObjectPoolLease<object> poolLease = pool.Lease();
			poolLease.Dispose();

			_ = poolLease.Value;
		};

		// Assert.
		act.Should().Throw<ObjectDisposedException>();
	}
}

sealed file class TestObjectPool : ObjectPool<object>
{
	public bool IsReturned { get; private set; }

	public override object Get() => new();
	public override void Return(object obj) => IsReturned = true;
}

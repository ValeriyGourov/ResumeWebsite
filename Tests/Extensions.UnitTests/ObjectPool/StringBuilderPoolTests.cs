using System.Text;

using AutoFixture;

using Extensions.ObjectPool;

using FluentAssertions;

namespace Extensions.UnitTests.ObjectPool;

[TestClass]
internal class StringBuilderPoolTests
{
	private static readonly Fixture _fixture = new();

	[TestMethod("Экземпляры StringBuilder не должны возвращаться в пул если их вместимость превыщает допустимую")]
	public void Return1()
	{
		// Arrange.
		const int maximumRetainedCapacity = 2;
		StringBuilderPool pool = new(1, 1, maximumRetainedCapacity);
		string bigString = new([.. _fixture.CreateMany<char>(maximumRetainedCapacity + 1)]);

		StringBuilder stringBuilder1 = pool.Get();
		_ = stringBuilder1.Append(bigString);
		pool.Return(stringBuilder1);

		// Act.
		StringBuilder stringBuilder2 = pool.Get();

		// Assert.
		_ = stringBuilder1.Should().NotBeSameAs(stringBuilder2);
	}

	[TestMethod("Должна возвращаться сформированная строка")]
	public void BuildString1()
	{
		// Arrange.
		string
			str1 = _fixture.Create<string>(),
			str2 = _fixture.Create<string>(),
			expected = str1 + str2;

		// Act.
		string actual = StringBuilderPool.BuildString(sb => sb
			.Append(str1)
			.Append(str2));

		// Assert.
		_ = actual.Should().Be(expected);
	}
}

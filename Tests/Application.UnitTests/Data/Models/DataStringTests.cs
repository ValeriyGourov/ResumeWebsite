using Application.Data.Models;

using AutoFixture;

using Localization.Infrastructure;

namespace Application.UnitTests.Data.Models;

[TestClass]
internal class DataStringTests
{
	private static readonly Fixture _fixture = new();

	public static IEnumerable<(string, Func<DataString, string>)> ToString1TestData { get; } =
	[
		("ru", static dataString => dataString.Ru),
		("ru-RU", static dataString => dataString.Ru),
		("en", static dataString => dataString.En),
		("en-US", static dataString => dataString.En),
		("fr", static dataString => dataString.En),
	];

	[TestMethod(DisplayName = "Преобразование в строку должно учитывать текущий язык")]
	[DynamicData(nameof(ToString1TestData))]
	public void ToString1(string cultureName, Func<DataString, string> expectedFactory)
	{
		ArgumentNullException.ThrowIfNull(expectedFactory);

		// Arrange.
		DataString value = _fixture.Create<DataString>();

		CultureChanger cultureChanger = new();
		cultureChanger.ChangeCulture(cultureName);

		string expected = expectedFactory(value);

		// Act.
		string actual = value.ToString();

		// Assert.
		_ = actual.Should().Be(expected);
	}

	public static IEnumerable<(DataString?, string?)> OperatorString1TestData()
	{
		yield return (null, null);

		DataString value = _fixture.Create<DataString>();
		yield return (value, value.Ru);
	}

	[TestMethod(DisplayName = "Неявное преобразование в строку должно учитывать значение null")]
	[DynamicData(nameof(OperatorString1TestData))]
	public void OperatorString1(DataString? value, string? expected)
	{
		// Arrange.
		CultureChanger cultureChanger = new();
		cultureChanger.ChangeCulture("ru");

		// Act.
		string? actual = value;

		// Assert.
		_ = actual.Should().Be(expected);
	}
}

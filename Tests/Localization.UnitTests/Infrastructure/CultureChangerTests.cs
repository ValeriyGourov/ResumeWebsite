#pragma warning disable CA1515

using System.Globalization;

using Localization.Infrastructure;

namespace Localization.UnitTests.Infrastructure;

[TestClass, DoNotParallelize]
public class CultureChangerTests
{
	[TestMethod]
	[TestProperty(TestProperties.MethodName, nameof(CultureChanger.ChangeCulture))]
	public void ThrowsArgumentNullExceptionIfCultureIsNull()
	{
		// Arrange.
		CultureChanger cultureChanger = new();

		// Act.
#pragma warning disable CS8625
		Action act = () => cultureChanger.ChangeCulture(null, CultureInfo.CurrentUICulture);
#pragma warning restore CS8625

		// Assert.
		act.Should().ThrowExactly<ArgumentNullException>();
	}

	[TestMethod]
	[TestProperty(TestProperties.MethodName, nameof(CultureChanger.ChangeCulture))]
	public void ThrowsArgumentNullExceptionIfUiCultureIsNull()
	{
		// Arrange.
		CultureChanger cultureChanger = new();

		// Act.
#pragma warning disable CS8625
		Action act = () => cultureChanger.ChangeCulture(CultureInfo.CurrentCulture, null);
#pragma warning restore CS8625

		// Assert.
		act.Should().ThrowExactly<ArgumentNullException>();
	}

	[TestMethod]
	[TestProperty(TestProperties.MethodName, nameof(CultureChanger.ChangeCulture))]
	public void PropertyCurrentCultureMustBeEqualsParameterCulture()
	{
		// Arrange.
		CultureChanger cultureChanger = new();
		CultureInfo culture = new("en-US");
		CultureInfo uiCulture = new("es-MX");

		// Act.
		cultureChanger.ChangeCulture(culture, uiCulture);

		// Assert.
		CultureChanger.CurrentCulture.Should().Be(culture);
	}

	[TestMethod]
	[TestProperty(TestProperties.MethodName, nameof(CultureChanger.ChangeCulture))]
	public void PropertyCurrentUICultureMustBeEqualsParameterUiCulture()
	{
		// Arrange.
		CultureChanger cultureChanger = new();
		CultureInfo culture = new("en-US");
		CultureInfo uiCulture = new("es-MX");

		// Act.
		cultureChanger.ChangeCulture(culture, uiCulture);

		// Assert.
		CultureChanger.CurrentUICulture.Should().Be(uiCulture);
	}

	[TestMethod]
	[TestProperty(TestProperties.MethodName, nameof(CultureChanger.ChangeCulture))]
	public void CultureInfoCurrentCultureMustBeEqualsParameterCulture()
	{
		// Arrange.
		CultureChanger cultureChanger = new();
		CultureInfo culture = new("en-US");
		CultureInfo uiCulture = new("es-MX");

		// Act.
		cultureChanger.ChangeCulture(culture, uiCulture);

		// Assert.
		CultureInfo.CurrentCulture.Should().Be(culture);
	}

	[TestMethod]
	[TestProperty(TestProperties.MethodName, nameof(CultureChanger.ChangeCulture))]
	public void CultureInfoCurrentUICultureMustBeEqualsParameterUiCulture()
	{
		// Arrange.
		CultureChanger cultureChanger = new();
		CultureInfo culture = new("en-US");
		CultureInfo uiCulture = new("es-MX");

		// Act.
		cultureChanger.ChangeCulture(culture, uiCulture);

		// Assert.
		CultureInfo.CurrentUICulture.Should().Be(uiCulture);
	}

	[TestMethod]
	[TestProperty(TestProperties.MethodName, nameof(CultureChanger.ChangeCulture))]
	public void PropertiesCurrentCultureMustBeSetByStringCultureName()
	{
		// Arrange.
		CultureChanger cultureChanger = new();
		const string cultureName = "es-MX";
		CultureInfo culture = new(cultureName);

		// Act.
		cultureChanger.ChangeCulture(cultureName);

		// Assert.
		CultureChanger.CurrentCulture.Should().Be(culture);
		CultureChanger.CurrentUICulture.Should().Be(culture);
	}

	[TestMethod]
	[TestProperty(TestProperties.MethodName, nameof(CultureChanger.ChangeCulture))]
	public void PropertiesCurrentCultureMustBeSetByOneParameterCulture()
	{
		// Arrange.
		CultureChanger cultureChanger = new();
		CultureInfo culture = new("es-MX");

		// Act.
#pragma warning disable CA1304
		cultureChanger.ChangeCulture(culture);
#pragma warning restore CA1304

		// Assert.
		CultureChanger.CurrentCulture.Should().Be(culture);
		CultureChanger.CurrentUICulture.Should().Be(culture);
	}

	[TestMethod]
	[TestProperty(TestProperties.MethodName, nameof(CultureChanger.ChangeCulture))]
	public void PropertiesCurrentCultureMustBeSetByStringCultureNames()
	{
		// Arrange.
		CultureChanger cultureChanger = new();
		const string cultureName = "en-US";
		const string cultureUiName = "es-MX";
		CultureInfo culture = new(cultureName);
		CultureInfo uiCulture = new(cultureUiName);

		// Act.
		cultureChanger.ChangeCulture(cultureName, cultureUiName);

		// Assert.
		CultureChanger.CurrentCulture.Should().Be(culture);
		CultureChanger.CurrentUICulture.Should().Be(uiCulture);
	}

	[TestMethod]
	[TestProperty(TestProperties.MethodName, nameof(CultureChanger.ChangeCulture))]
	[TestProperty(TestProperties.EventName, nameof(CultureChanger.CultureChanged))]
	public void EventMustBeInvokedWhenCultureChanged()
	{
		// Arrange.
		CultureChanger cultureChanger = new();

		CultureInfo culture = new("en-US");
		CultureInfo uiCulture = new("es-MX");

		bool invoked = false;
		cultureChanger.CultureChanged += (sender, e) => invoked = true;

		// Act.
		cultureChanger.ChangeCulture(culture, uiCulture);

		// Assert.
		invoked.Should().BeTrue(
			"Событие '{0}' не вызвано.",
			nameof(CultureChanger.CultureChanged));
	}
}

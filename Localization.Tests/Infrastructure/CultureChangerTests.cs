using System;
using System.Globalization;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TestInfrastructure;

namespace Localization.Infrastructure.Tests
{
	[TestClass]
	public class CultureChangerTests
	{
		[TestMethod]
		[TestProperty(TestProperties.MethodName, nameof(CultureChanger.ChangeCulture))]
		public void ThrowsArgumentNullExceptionIfCultureIsNull()
		{
			// Arrange.
			CultureChanger cultureChanger = new CultureChanger();

			// Act.
#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
			void act() => cultureChanger.ChangeCulture(null, CultureInfo.CurrentUICulture);
#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.

			// Assert.
			Assert.ThrowsException<ArgumentNullException>(act);
		}

		[TestMethod]
		[TestProperty(TestProperties.MethodName, nameof(CultureChanger.ChangeCulture))]
		public void ThrowsArgumentNullExceptionIfUiCultureIsNull()
		{
			// Arrange.
			CultureChanger cultureChanger = new CultureChanger();

			// Act.
#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
			void act() => cultureChanger.ChangeCulture(CultureInfo.CurrentCulture, null);
#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.

			// Assert.
			Assert.ThrowsException<ArgumentNullException>(act);
		}

		[TestMethod]
		[TestProperty(TestProperties.MethodName, nameof(CultureChanger.ChangeCulture))]
		public void PropertyCurrentCultureMustBeEqualsParameterCulture()
		{
			// Arrange.
			CultureChanger cultureChanger = new CultureChanger();
			CultureInfo culture = new CultureInfo("en-US");
			CultureInfo uiCulture = new CultureInfo("es-MX");

			// Act.
			cultureChanger.ChangeCulture(culture, uiCulture);

			// Assert.
			Assert.AreEqual(culture, CultureChanger.CurrentCulture);
		}

		[TestMethod]
		[TestProperty(TestProperties.MethodName, nameof(CultureChanger.ChangeCulture))]
		public void PropertyCurrentUICultureMustBeEqualsParameterUiCulture()
		{
			// Arrange.
			CultureChanger cultureChanger = new CultureChanger();
			CultureInfo culture = new CultureInfo("en-US");
			CultureInfo uiCulture = new CultureInfo("es-MX");

			// Act.
			cultureChanger.ChangeCulture(culture, uiCulture);

			// Assert.
			Assert.AreEqual(uiCulture, CultureChanger.CurrentUICulture);
		}

		[TestMethod]
		[TestProperty(TestProperties.MethodName, nameof(CultureChanger.ChangeCulture))]
		public void CultureInfoCurrentCultureMustBeEqualsParameterCulture()
		{
			// Arrange.
			CultureChanger cultureChanger = new CultureChanger();
			CultureInfo culture = new CultureInfo("en-US");
			CultureInfo uiCulture = new CultureInfo("es-MX");

			// Act.
			cultureChanger.ChangeCulture(culture, uiCulture);

			// Assert.
			Assert.AreEqual(culture, CultureInfo.CurrentCulture);
		}

		[TestMethod]
		[TestProperty(TestProperties.MethodName, nameof(CultureChanger.ChangeCulture))]
		public void CultureInfoCurrentUICultureMustBeEqualsParameterUiCulture()
		{
			// Arrange.
			CultureChanger cultureChanger = new CultureChanger();
			CultureInfo culture = new CultureInfo("en-US");
			CultureInfo uiCulture = new CultureInfo("es-MX");

			// Act.
			cultureChanger.ChangeCulture(culture, uiCulture);

			// Assert.
			Assert.AreEqual(uiCulture, CultureInfo.CurrentUICulture);
		}

		[TestMethod]
		[TestProperty(TestProperties.MethodName, nameof(CultureChanger.ChangeCulture))]
		public void PropertiesCurrentCultureMustBeSetByStringCultureName()
		{
			// Arrange.
			CultureChanger cultureChanger = new CultureChanger();
			string cultureName = "es-MX";
			CultureInfo culture = new CultureInfo(cultureName);

			// Act.
			cultureChanger.ChangeCulture(cultureName);

			// Assert.
			Assert.AreEqual(culture, CultureChanger.CurrentCulture);
			Assert.AreEqual(culture, CultureChanger.CurrentUICulture);
		}

		[TestMethod]
		[TestProperty(TestProperties.MethodName, nameof(CultureChanger.ChangeCulture))]
		public void PropertiesCurrentCultureMustBeSetByOneParameterCulture()
		{
			// Arrange.
			CultureChanger cultureChanger = new CultureChanger();
			CultureInfo culture = new CultureInfo("es-MX");

			// Act.
#pragma warning disable CA1304 // Укажите CultureInfo
			cultureChanger.ChangeCulture(culture);
#pragma warning restore CA1304 // Укажите CultureInfo

			// Assert.
			Assert.AreEqual(culture, CultureChanger.CurrentCulture);
			Assert.AreEqual(culture, CultureChanger.CurrentUICulture);
		}

		[TestMethod]
		[TestProperty(TestProperties.MethodName, nameof(CultureChanger.ChangeCulture))]
		public void PropertiesCurrentCultureMustBeSetByStringCultureNames()
		{
			// Arrange.
			CultureChanger cultureChanger = new CultureChanger();
			string cultureName = "en-US";
			string cultureUiName = "es-MX";
			CultureInfo culture = new CultureInfo(cultureName);
			CultureInfo uiCulture = new CultureInfo(cultureUiName);

			// Act.
			cultureChanger.ChangeCulture(cultureName, cultureUiName);

			// Assert.
			Assert.AreEqual(culture, CultureChanger.CurrentCulture);
			Assert.AreEqual(uiCulture, CultureChanger.CurrentUICulture);
		}

		[TestMethod]
		[TestProperty(TestProperties.MethodName, nameof(CultureChanger.ChangeCulture))]
		[TestProperty(TestProperties.EventName, nameof(CultureChanger.CultureChanged))]
		public void EventMustBeInvokedWhenCultureChanged()
		{
			// Arrange.
			CultureChanger cultureChanger = new CultureChanger();

			CultureInfo culture = new CultureInfo("en-US");
			CultureInfo uiCulture = new CultureInfo("es-MX");

			bool invoked = false;
			Task CultureChanger_CultureChanged()
			{
				invoked = true;
				return Task.CompletedTask;
			}

			cultureChanger.CultureChanged += CultureChanger_CultureChanged;

			// Act.
			cultureChanger.ChangeCulture(culture, uiCulture);

			// Assert.
			Assert.IsTrue(invoked, "Событие '{0}' не вызвано.", nameof(CultureChanger.CultureChanged));
		}
	}
}
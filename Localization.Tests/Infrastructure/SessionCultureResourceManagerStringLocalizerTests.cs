﻿using System;
using System.Globalization;
using System.Resources;

using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using TestInfrastructure;

namespace Localization.Infrastructure.Tests
{
	[TestClass]
	public class SessionCultureResourceManagerStringLocalizerTests
	{
		[TestMethod]
		[TestProperty(TestProperties.Constructor, null)]
		public void ThrowsArgumentNullExceptionInConstructorIfLoggerIsNull()
		{
			// Arrange.
			ResourceManager resourceManager = Mock.Of<ResourceManager>();
			ILogger? logger = null;

			// Act.
			void act()
			{
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
				using SessionCultureResourceManagerStringLocalizer localizer = new SessionCultureResourceManagerStringLocalizer(resourceManager, logger);
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
			};

			// Assert.
			Assert.ThrowsException<ArgumentNullException>(act);
		}

		#region this[string name]

		[TestMethod]
		[TestProperty(TestProperties.IndexerSignature, "string")]
		public void ThrowsArgumentNullExceptionByNameIfNameIsNull()
		{
			// Arrange.
			const string? name = null;

			using SessionCultureResourceManagerStringLocalizer localizer = GetLocalizer();

			// Act.
			void act()
			{
#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
				LocalizedString actual = localizer[name];
#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
			};

			// Assert.
			Assert.ThrowsException<ArgumentNullException>(act);
		}

		[TestMethod]
		[TestProperty(TestProperties.IndexerSignature, "string")]
		public void ReturnsLocalizedStringByNameForChangedCultureAndExistingResourceString()
		{
			// Arrange.
			const string name = "TestString";
			CultureInfo culture = new CultureInfo("es-MX");
			const string localizedString = "Test string";

			ChangeCulture(culture);
			using SessionCultureResourceManagerStringLocalizer localizer = GetLocalizer(name, culture, localizedString);

			// Act.
			LocalizedString actual = localizer[name];

			// Assert.
			Assert.AreEqual(localizedString, actual.Value);
		}

		[TestMethod]
		[TestProperty(TestProperties.IndexerSignature, "string")]
		public void ReturnsLocalizedStringNameByNameForChangedCultureAndNonexistentResourceString()
		{
			// Arrange.
			const string name = "TestString";
			CultureInfo culture = new CultureInfo("es-MX");
			const string? localizedString = null;

			ChangeCulture(culture);
			using SessionCultureResourceManagerStringLocalizer localizer = GetLocalizer(name, culture, localizedString);

			// Act.
			LocalizedString actual = localizer[name];

			// Assert.
			Assert.AreEqual(name, actual.Value);
		}

		[TestMethod]
		[TestProperty(TestProperties.IndexerSignature, "string")]
		public void ShouldReturnLocalizedStringAfrerSwitchingFromNonExistentCulture()
		{
			// Arrange.
			const string name = "TestString";
			CultureInfo existingCulture = new CultureInfo("es-MX");
			CultureInfo nonExistentCulture = new CultureInfo("ru-RU");
			const string localizedString = "Test string";

			Mock<ResourceManager> resourceManagerMock = new Mock<ResourceManager>();
			resourceManagerMock
				.Setup(resourceManager => resourceManager.GetString(name, existingCulture))
				.Returns(localizedString);
			resourceManagerMock
				.Setup(resourceManager => resourceManager.GetString(name, nonExistentCulture))
				.Throws<MissingManifestResourceException>();
			using SessionCultureResourceManagerStringLocalizer localizer = GetLocalizer(resourceManagerMock.Object);

			// Act.
			ChangeCulture(nonExistentCulture);
			LocalizedString LocalizedStringForNonExistentCulture = localizer[name];

			ChangeCulture(existingCulture);
			LocalizedString LocalizedStringForNonExistingCulture = localizer[name];

			// Assert.
			Assert.AreEqual(name, LocalizedStringForNonExistentCulture.Value);
			Assert.AreEqual(localizedString, LocalizedStringForNonExistingCulture.Value);
		}

		#endregion

		#region this[string name, params object[] arguments]

		[TestMethod]
		[TestProperty(TestProperties.IndexerSignature, "string, params object[]")]
		public void ThrowsArgumentNullExceptionByNameWithParamsIfNameIsNull()
		{
			// Arrange.
			const string? name = null;
			const int param = 0;

			using SessionCultureResourceManagerStringLocalizer localizer = GetLocalizer();

			// Act.
			void act()
			{
#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
				LocalizedString actual = localizer[name, param];
#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
			};

			// Assert.
			Assert.ThrowsException<ArgumentNullException>(act);
		}

		[TestMethod]
		[TestProperty(TestProperties.IndexerSignature, "string, params object[]")]
		public void ReturnsLocalizedStringByNameWithParamsForChangedCultureAndExistingResourceString()
		{
			// Arrange.
			const string name = "TestString";
			CultureInfo culture = new CultureInfo("es-MX");
			const string localizedString = "Test string {0}";
			const int param = 0;
			const string expected = "Test string 0";

			ChangeCulture(culture);
			using SessionCultureResourceManagerStringLocalizer localizer = GetLocalizer(name, culture, localizedString);

			// Act.
			LocalizedString actual = localizer[name, param];

			// Assert.
			Assert.AreEqual(expected, actual.Value);
		}

		[TestMethod]
		[TestProperty(TestProperties.IndexerSignature, "string, params object[]")]
		public void ReturnsLocalizedStringNameByNameWithParamsForChangedCultureAndNonexistentResourceString()
		{
			// Arrange.
			const string name = "TestString {0}";
			CultureInfo culture = new CultureInfo("es-MX");
			const string? localizedString = null;
			const int param = 0;
			const string expected = "TestString 0";

			using SessionCultureResourceManagerStringLocalizer localizer = GetLocalizer(name, culture, localizedString);
			ChangeCulture(culture);

			// Act.
			LocalizedString actual = localizer[name, param];

			// Assert.
			Assert.AreEqual(expected, actual.Value);
		}

		#endregion

		#region GetAllStrings

		//[TestMethod]
		//[TestProperty(TestProperties.MethodName, nameof(SessionCultureResourceManagerStringLocalizer.GetAllStrings))]
		//public void ShouldReturnEmptyCollectionIfResourcesNotFound()
		//{
		//	// Arrange.
		//	using SessionCultureResourceManagerStringLocalizer localizer = GetLocalizer(null);

		//	// Act.
		//	IEnumerable<LocalizedString> localizedStrings = localizer.GetAllStrings();

		//	// Assert.
		//	Assert.IsFalse(localizedStrings.Any(), "Если ресурсы отсутствуют, коллекция локализованных строк должна быть пустой.");
		//}

		//[TestMethod]
		//[TestProperty(TestProperties.MethodName, nameof(SessionCultureResourceManagerStringLocalizer.GetAllStrings))]
		//public void MyTestMethod()
		//{
		//	// Arrange.
		//	//IEnumerable<DictionaryEntry> resources = new List<DictionaryEntry>()
		//	//{
		//	//	new DictionaryEntry("TestString", "Test string"),
		//	//	new DictionaryEntry("TestString2", "Test string 2 {0}"),
		//	//};
		//	Dictionary<object, object?> resources = new Dictionary<object, object?>()
		//	{
		//		{ "TestString", "Test string" },
		//		{ "TestString2", "Test string 2 {0}" },
		//	};
		//	//Dictionary<string, string>.Enumerator dd = resources.GetEnumerator();
		//	//IDictionaryEnumerator dictionaryEnumerator = Mock.Of<IDictionaryEnumerator>(dictionaryEnumerator => dictionaryEnumerator.Value == (object?)new DictionaryEntry("TestString", "Test string"));
		//	//ResourceSet resourceSet = Mock.Of<ResourceSet>(resourceSet => resourceSet.GetEnumerator() == dictionaryEnumerator);
		//	IDictionaryEnumerator dictionaryEnumerator = resources.GetEnumerator() /*as IDictionaryEnumerator*/;
		//	ResourceSet resourceSet = Mock.Of<ResourceSet>(resourceSet => resourceSet.GetEnumerator() == dictionaryEnumerator);

		//	CultureInfo resourceCulture = new CultureInfo("ru");
		//	CultureInfo currentCulture = new CultureInfo("es");
		//	ResourceManager resourceManager = Mock.Of<ResourceManager>(
		//		resourceManager => resourceManager.GetResourceSet(resourceCulture, false, true) == resourceSet);

		//	//foreach (var item in resourceSet)
		//	//{

		//	//}
		//	ChangeCulture(resourceCulture);
		//	//ChangeCulture(currentCulture);
		//	using SessionCultureResourceManagerStringLocalizer localizer = GetLocalizer(resourceManager);

		//	// Act.
		//	//IEnumerable<LocalizedString> localizedStrings = localizer.GetAllStrings(true);
		//	LocalizedString[] localizedStrings = localizer.GetAllStrings().ToArray();
		//	//var ff = localizer["TestString"];
		//	//var gg = localizer.WithCulture(resourceCulture)["TestString"];

		//	// Assert.
		//	CollectionAssert.AreEquivalent(
		//		resources.Select(item => item.Key).ToArray(),
		//		localizedStrings.Select(item => item.Name).ToArray());
		//	//CollectionAssert.AreEquivalent(
		//	//	resources.Keys.ToArray(),
		//	//	localizedStrings.Select(item => item.Name).ToArray());
		//}

		#endregion

		#region Вспомогательные методы

		private SessionCultureResourceManagerStringLocalizer GetLocalizer()
		{
			ResourceManager resourceManager = Mock.Of<ResourceManager>();
			return GetLocalizer(resourceManager);
		}

		private SessionCultureResourceManagerStringLocalizer GetLocalizer(
			string name,
			CultureInfo culture,
			string? localizedString)
		{
			ResourceManager resourceManager = Mock.Of<ResourceManager>(
				resourceManager => resourceManager.GetString(name, culture) == localizedString);
			return GetLocalizer(resourceManager);
		}

		private SessionCultureResourceManagerStringLocalizer GetLocalizer(
			ResourceManager resourceManager)
		{
			ILogger logger = Mock.Of<ILogger>();
			return new SessionCultureResourceManagerStringLocalizer(resourceManager, logger);
		}

		private void ChangeCulture(CultureInfo culture)
		{
			CultureChanger cultureChanger = new CultureChanger();
			cultureChanger.ChangeCulture(culture, culture);
		}

		#endregion
	}
}
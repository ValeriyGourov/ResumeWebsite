using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TestInfrastructure;

namespace Application.Infrastructure.Validation.Tests
{
	[TestClass]
	public class ValidateComplexTypeAttributeTests
	{
		[TestMethod]
		[TestProperty(TestProperties.MethodName, nameof(ValidateComplexTypeAttribute.IsValid))]
		public void ValidationReturnSuccessIfValueIsNull()
		{
			// Arrange.
			Person value = new Person
			{
				MainAddress = new Address
				{
					Street1 = "Awesome street",
					City = "Awesome Town",
					Zip = null
				},
				Name = "Josh"
			};

			// Act.
			List<ValidationResult> validationResults = new List<ValidationResult>();
			bool result = TryValidateObject(value, validationResults);

			// Assert.
			Assert.IsTrue(result);
		}

		[TestMethod]
		[TestProperty(TestProperties.MethodName, nameof(ValidateComplexTypeAttribute.IsValid))]
		public void ValidationThrowsArgumentNullExceptionIfValidationContextIsNull()
		{
			// Arrange.
			Person value = new Person();
			ValidationContext? validationContext = null;
			ValidateComplexTypeAttributeTest validateComplexTypeAttribute = new ValidateComplexTypeAttributeTest();

			// Act.
			void act()
			{
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
				validateComplexTypeAttribute.IsValid(value, validationContext);
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
			}

			// Assert.
			Assert.ThrowsException<ArgumentNullException>(act);
		}

		[TestMethod]
		[TestProperty(TestProperties.MethodName, nameof(ValidateComplexTypeAttribute.IsValid))]
		public void CollectionValidationFaledForSomeItems()
		{
			// Arrange.
			Person value = new Person
			{
				Name = "Josh",
				MainAddress = new Address
				{
					Street1 = "Awesome street",
					City = "Awesome Town",
					Zip = null
				},
#pragma warning disable CS8619 // Допустимость значения NULL для ссылочных типов в значении не соответствует целевому типу.
				AdditionalAddresses = new[]	// Здесь одна ошибка, так как один элемент коллекции содержит ошибки.
				{
					new Address
					{
						Street1 = "Street 1",
						City = "City 1"
					},
					null,	// Это значение должно быть просто пропущено при проверке.
					new Address()	// Здесь должно быть две ошибки проверки: Street1, City.
				}
#pragma warning restore CS8619 // Допустимость значения NULL для ссылочных типов в значении не соответствует целевому типу.
			};

			const int validationResultsExpectedCount = 1;
			const int additionalAddressesValidationResultsExpectedCount = 1;
			const int address3ValidationResultsExpectedCount = 2;

			// Act.
			List<ValidationResult> validationResults = new List<ValidationResult>();
			bool result = TryValidateObject(value, validationResults);

			ComplexTypeValidationResult? additionalAddressesLevel = validationResults[0] as ComplexTypeValidationResult;
			ValidationResult[]? additionalAddressesResults = additionalAddressesLevel?.ValidationResults.ToArray();
			ComplexTypeValidationResult? address3Level = additionalAddressesResults?[0] as ComplexTypeValidationResult;
			ValidationResult[]? address3Results = address3Level?.ValidationResults.ToArray();

			// Assert.
			Assert.IsFalse(result);
			Assert.AreEqual(validationResultsExpectedCount, validationResults.Count);
			Assert.AreEqual(
				additionalAddressesValidationResultsExpectedCount,
				additionalAddressesResults?.Length,
				$"Здесь должна быть ошибка проверки свойства: {nameof(Person.AdditionalAddresses)}.");
			Assert.AreEqual(
				address3ValidationResultsExpectedCount,
				address3Results?.Length,
				$"Здесь должны быть ошибки проверки свойств: {nameof(Address.Street1)}, {nameof(Address.City)}.");
			CollectionAssert.AllItemsAreInstancesOfType(validationResults, typeof(ComplexTypeValidationResult));
		}

		[TestMethod]
		[TestProperty(TestProperties.MethodName, nameof(ValidateComplexTypeAttribute.IsValid))]
		public void ObjectValidationFailedForInvalidProperties()
		{
			// Arrange.
			Person value = new Person
			{
				Name = "Josh",
				MainAddress = new Address() // Здесь должно быть две ошибки проверки: Street1, City.
			};

			const int validationResultsExpectedCount = 1;
			const int mainAddressValidationResultsExpectedCount = 2;

			// Act.
			List<ValidationResult> validationResults = new List<ValidationResult>();
			bool result = TryValidateObject(value, validationResults);

			ComplexTypeValidationResult? mainAddressLevel = validationResults[0] as ComplexTypeValidationResult;
			ValidationResult[]? mainAddressResults = mainAddressLevel?.ValidationResults.ToArray();

			// Assert.
			Assert.IsFalse(result);
			Assert.AreEqual(validationResultsExpectedCount, validationResults.Count);
			Assert.AreEqual(
				mainAddressValidationResultsExpectedCount,
				mainAddressResults?.Length,
				$"Здесь должны быть ошибки проверки свойств: {nameof(Address.Street1)}, {nameof(Address.City)}.");
			CollectionAssert.AllItemsAreInstancesOfType(validationResults, typeof(ComplexTypeValidationResult));
		}

		[TestMethod]
		[TestProperty(TestProperties.MethodName, nameof(ValidateComplexTypeAttribute.IsValid))]
		public void ValidationSuccessfulForValidData()
		{
			// Arrange.
			Person value = new Person
			{
				Name = "Josh",
				MainAddress = new Address
				{
					Street1 = "Awesome street",
					Street2 = "Awesome street 2",
					City = "Awesome Town",
					Zip = new ZipCode
					{
						PrimaryCode = "Primary Code",
						SubCode = "SubCode"
					}
				},
				AdditionalAddresses = new[]
				{
					new Address
					{
						Street1 = "Street 1",
						City = "City 1"
					},
					new Address
					{
						Street1 = "Street 2",
						City = "City 2"
					}
				}
			};

			const int validationResultsExpectedCount = 0;

			// Act.
			List<ValidationResult> validationResults = new List<ValidationResult>();
			bool result = TryValidateObject(value, validationResults);

			// Assert.
			Assert.IsTrue(result);
			Assert.AreEqual(validationResultsExpectedCount, validationResults.Count);
		}

		private static bool TryValidateObject(object value, List<ValidationResult> validationResults)
		{
			ValidationContext validationContext = new ValidationContext(value);
			return Validator.TryValidateObject(value, validationContext, validationResults, true);
		}

		private sealed class Person
		{
			[Required]
			public string Name { get; set; } = null!;

			[Required, ValidateComplexType]
			public Address MainAddress { get; set; } = null!;

			[ValidateComplexType]
			public IEnumerable<Address>? AdditionalAddresses { get; set; }
		}

		private sealed class Address
		{
			[Required]
			public string Street1 { get; set; } = null!;

			public string? Street2 { get; set; }

			[Required]
			public string City { get; set; } = null!;

			[ValidateComplexType]
			public ZipCode? Zip { get; set; }
		}

		private sealed class ZipCode
		{
			[Required]
			public string PrimaryCode { get; set; } = null!;

			public string? SubCode { get; set; }
		}

		private sealed class ValidateComplexTypeAttributeTest : ValidateComplexTypeAttribute
		{
			public new ValidationResult? IsValid(object value, ValidationContext validationContext) => base.IsValid(value, validationContext);
		}
	}
}

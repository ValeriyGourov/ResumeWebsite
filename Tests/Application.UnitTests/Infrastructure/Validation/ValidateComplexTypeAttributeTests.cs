#pragma warning disable CA1515

using Application.Infrastructure.Validation;

namespace Application.UnitTests.Infrastructure.Validation;

[TestClass]
public class ValidateComplexTypeAttributeTests
{
	[TestMethod]
	[TestProperty(TestProperties.MethodName, nameof(ValidateComplexTypeAttribute.IsValid))]
	public void ValidationReturnSuccessIfValueIsNull()
	{
		// Arrange.
		Person value = new()
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
		List<ValidationResult> validationResults = [];
		bool result = TryValidateObject(value, validationResults);

		// Assert.
		result.Should().BeTrue();
	}

	[TestMethod]
	[TestProperty(TestProperties.MethodName, nameof(ValidateComplexTypeAttribute.IsValid))]
	public void ValidationThrowsArgumentNullExceptionIfValidationContextIsNull()
	{
		// Arrange.
		Person value = new();
		ValidationContext? validationContext = null;
		TestValidateComplexTypeAttribute validateComplexTypeAttribute = new();

		// Act.
#pragma warning disable CS8604
		Action act = () => _ = validateComplexTypeAttribute.IsValid(value, validationContext);
#pragma warning restore CS8604

		// Assert.
		act.Should().ThrowExactly<ArgumentNullException>()
			.WithParameterName(nameof(validationContext));
	}

	[TestMethod]
	[TestProperty(TestProperties.MethodName, nameof(ValidateComplexTypeAttribute.IsValid))]
	public void CollectionValidationFaledForSomeItems()
	{
		// Arrange.
		Person value = new()
		{
			Name = "Josh",
			MainAddress = new Address
			{
				Street1 = "Awesome street",
				City = "Awesome Town",
				Zip = null
			},
			// Здесь одна ошибка, так как один элемент коллекции содержит ошибки.
			AdditionalAddresses =
			[
				new Address
				{
					Street1 = "Street 1",
					City = "City 1"
				},
				null!,	// Это значение должно быть просто пропущено при проверке.
				new Address()	// Здесь должно быть две ошибки проверки: Street1, City.
			]
		};

		const int validationResultsExpectedCount = 1;
		const int additionalAddressesValidationResultsExpectedCount = 1;
		const int address3ValidationResultsExpectedCount = 2;

		// Act.
		List<ValidationResult> validationResults = [];
		bool result = TryValidateObject(value, validationResults);

		ComplexTypeValidationResult? additionalAddressesLevel = validationResults[0] as ComplexTypeValidationResult;
		ValidationResult[]? additionalAddressesResults = additionalAddressesLevel?.ValidationResults.ToArray();
		ComplexTypeValidationResult? address3Level = additionalAddressesResults?[0] as ComplexTypeValidationResult;
		ValidationResult[]? address3Results = address3Level?.ValidationResults.ToArray();

		// Assert.
		result.Should().BeFalse();
		validationResults.Should()
			.HaveCount(validationResultsExpectedCount)
			.And.AllBeOfType<ComplexTypeValidationResult>();

		additionalAddressesResults.Should().HaveCount(
			additionalAddressesValidationResultsExpectedCount,
			$"Здесь должна быть ошибка проверки свойства: {nameof(Person.AdditionalAddresses)}.");

		address3Results.Should().HaveCount(
			address3ValidationResultsExpectedCount,
			$"Здесь должны быть ошибки проверки свойств: {nameof(Address.Street1)}, {nameof(Address.City)}.");
	}

	[TestMethod]
	[TestProperty(TestProperties.MethodName, nameof(ValidateComplexTypeAttribute.IsValid))]
	public void ObjectValidationFailedForInvalidProperties()
	{
		// Arrange.
		Person value = new()
		{
			Name = "Josh",
			MainAddress = new Address() // Здесь должно быть две ошибки проверки: Street1, City.
		};

		const int validationResultsExpectedCount = 1;
		const int mainAddressValidationResultsExpectedCount = 2;

		// Act.
		List<ValidationResult> validationResults = [];
		bool result = TryValidateObject(value, validationResults);

		ComplexTypeValidationResult? mainAddressLevel = validationResults[0] as ComplexTypeValidationResult;
		ValidationResult[]? mainAddressResults = mainAddressLevel?.ValidationResults.ToArray();

		// Assert.
		result.Should().BeFalse();
		validationResults.Should()
			.HaveCount(validationResultsExpectedCount)
			.And.AllBeOfType<ComplexTypeValidationResult>();

		mainAddressResults.Should().HaveCount(
			mainAddressValidationResultsExpectedCount,
			$"Здесь должны быть ошибки проверки свойств: {nameof(Address.Street1)}, {nameof(Address.City)}.");
	}

	[TestMethod]
	[TestProperty(TestProperties.MethodName, nameof(ValidateComplexTypeAttribute.IsValid))]
	public void ValidationSuccessfulForValidData()
	{
		// Arrange.
		Person value = new()
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
			AdditionalAddresses =
			[
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
			]
		};

		const int validationResultsExpectedCount = 0;

		// Act.
		List<ValidationResult> validationResults = [];
		bool result = TryValidateObject(value, validationResults);

		// Assert.
		result.Should().BeTrue();
		validationResults.Should().HaveCount(validationResultsExpectedCount);
	}

	private static bool TryValidateObject(object value, List<ValidationResult> validationResults)
	{
		ValidationContext validationContext = new(value);
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

#pragma warning disable S3993
	private sealed class TestValidateComplexTypeAttribute : ValidateComplexTypeAttribute
#pragma warning restore S3993
	{
		public new ValidationResult? IsValid(object value, ValidationContext validationContext) => base.IsValid(value, validationContext);
	}
}

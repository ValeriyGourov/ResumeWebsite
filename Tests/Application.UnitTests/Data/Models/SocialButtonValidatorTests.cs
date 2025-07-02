using System.Linq.Expressions;

using Application.Data.Models;

using AutoFixture;

namespace Application.UnitTests.Data.Models;

[TestClass]
internal class SocialButtonValidatorTests : ModelValidatorTestsBase<SocialButton, SocialButtonValidator>
{
	public static IEnumerable<(SocialButton, Expression<Func<SocialButton, object>>, string)> ValidateWithInvalidValueTestData()
	{
		yield return (
			new SocialButton(
				Uri: null!,
				IconName: Fixture.Create<string>()),
			item => item.Uri,
			NotNullValidatorName);

		yield return (
			new SocialButton(
				Uri: Fixture.Create<Uri>(),
				IconName: null!),
			item => item.IconName,
			NotEmptyValidatorName);

		yield return (
			new SocialButton(
				Uri: Fixture.Create<Uri>(),
				IconName: string.Empty),
			item => item.IconName,
			NotEmptyValidatorName);
	}

	[TestMethod(ValidateWithIncorrectDataDisplayName)]
	[DynamicData(nameof(ValidateWithInvalidValueTestData))]
	public override void ValidateWithInvalidValue(
		SocialButton value,
		Expression<Func<SocialButton, object>> property,
		string expectedErrorCode)
		=> base.ValidateWithInvalidValue(value, property, expectedErrorCode);
}

#pragma warning disable CA1515

using System.Security.Cryptography;

using Application.Data.Models;

using AutoFixture;

namespace Application.UnitTests.Data.Models;

[TestClass]
internal class ResumeDataValidatorTests : ModelValidatorTestsBase<ResumeData, ResumeDataValidator>
{
	static ResumeDataValidatorTests()
	{
		Fixture.Customize<MonthYearTimeLineItem>(static transformation =>
		{
			DateOnly startDate = Fixture.Create<DateOnly>();
			DateOnly endDate = startDate.AddYears(RandomNumberGenerator.GetInt32(1, 100));

			return transformation
				.With(item => item.StartDate, startDate)
				.With(item => item.EndDate, endDate);
		});

		Fixture.Customize<YearTimeLineItem>(static transformation =>
		{
			int startYear = Fixture.Create<int>();
			int endYear = startYear + RandomNumberGenerator.GetInt32(1, 100);

			return transformation
				.With(item => item.StartYear, startYear)
				.With(item => item.EndYear, endYear);
		});

		Fixture.Customize<SkillItem>(static transformation
			=> transformation.With(
				item => item.Percent,
				RandomNumberGenerator.GetInt32(1, 100)));
	}

	public static IEnumerable<(ResumeData, string, string)> ValidateWithInvalidValueTestData()
	{
		DataString invalidDataString = new(
			En: null!,
			Ru: Fixture.Create<string>());

		yield return (
			new ResumeData(
				Name: invalidDataString,
				Surname: Fixture.Create<DataString>(),
				Title: Fixture.Create<DataString>()),
			GetPropertyName(item => item.Name.En),
			NotEmptyValidatorName);

		yield return (
			new ResumeData(
				Name: Fixture.Create<DataString>(),
				Surname: invalidDataString,
				Title: Fixture.Create<DataString>()),
			GetPropertyName(item => item.Surname.En),
			NotEmptyValidatorName);

		yield return (
			new ResumeData(
				Name: Fixture.Create<DataString>(),
				Surname: Fixture.Create<DataString>(),
				Title: invalidDataString),
			GetPropertyName(item => item.Title.En),
			NotEmptyValidatorName);

		yield return (
			new ResumeData(
				Name: Fixture.Create<DataString>(),
				Surname: Fixture.Create<DataString>(),
				Title: Fixture.Create<DataString>(),
				SocialButtons: [new(
					Uri: null!,
					IconName: Fixture.Create<string>())]),
			GetCollectionItemPropertyName(
				item => item.SocialButtons,
				item => item.Uri),
			NotNullValidatorName);

		yield return (
			new ResumeData(
				Name: Fixture.Create<DataString>(),
				Surname: Fixture.Create<DataString>(),
				Title: Fixture.Create<DataString>(),
				Contacts: [new(
					Title: invalidDataString ,
					Description: Fixture.Create<DataString>(),
					Hyperlink: null)]),
			GetCollectionItemPropertyName(
				item => item.Contacts,
				item => item.Title.En),
			NotEmptyValidatorName);

		yield return (
			new ResumeData(
				Name: Fixture.Create<DataString>(),
				Surname: Fixture.Create<DataString>(),
				Title: Fixture.Create<DataString>(),
				Intro: invalidDataString),
			GetPropertyName(item => item.Intro!.En),
			NotEmptyValidatorName);

		yield return (
			new ResumeData(
				Name: Fixture.Create<DataString>(),
				Surname: Fixture.Create<DataString>(),
				Title: Fixture.Create<DataString>(),
				Expertise: [new(
					Title: invalidDataString,
					Description: Fixture.Create<DataString>())]),
			GetCollectionItemPropertyName(
				item => item.Expertise!,
				item => item.Title.En),
			NotEmptyValidatorName);

		yield return (
			new ResumeData(
				Name: Fixture.Create<DataString>(),
				Surname: Fixture.Create<DataString>(),
				Title: Fixture.Create<DataString>(),
				Achievements: [invalidDataString]),
			GetCollectionItemPropertyName(
				item => item.Achievements!,
				item => item.En),
			NotEmptyValidatorName);

		yield return (
			new ResumeData(
				Name: Fixture.Create<DataString>(),
				Surname: Fixture.Create<DataString>(),
				Title: Fixture.Create<DataString>(),
				Skills: [new(
					Title: Fixture.Create<DataString>(),
					Percent: 0)]),
			GetCollectionItemPropertyName(
				item => item.Skills!,
				item => item.Percent),
			InclusiveBetweenValidatorName);

		yield return (
			new ResumeData(
				Name: Fixture.Create<DataString>(),
				Surname: Fixture.Create<DataString>(),
				Title: Fixture.Create<DataString>(),
				Experience: [new(
					Institution: Fixture.Create<DataString>(),
					Position: Fixture.Create<DataString>(),
					Location: Fixture.Create<DataString>(),
					Projects: Fixture.CreateMany<ExperienceProject>(),
					StartDate: DateOnly.MinValue,
					EndDate: null)]),
			GetCollectionItemPropertyName(
				item => item.Experience!,
				item => item.StartDate),
			NotEmptyValidatorName);

		yield return (
			new ResumeData(
				Name: Fixture.Create<DataString>(),
				Surname: Fixture.Create<DataString>(),
				Title: Fixture.Create<DataString>(),
				Education: [new(
					Institution: Fixture.Create<DataString>(),
					Position: Fixture.Create<DataString>(),
					Location: Fixture.Create<DataString>(),
					Description: Fixture.Create<DataString>(),
					StartYear: 0,
					EndYear: null)]),
			GetCollectionItemPropertyName(
				item => item.Education!,
				item => item.StartYear),
			GreaterThanValidatorName);

		yield return (
			new ResumeData(
				Name: Fixture.Create<DataString>(),
				Surname: Fixture.Create<DataString>(),
				Title: Fixture.Create<DataString>(),
				Profiles: [new(
					Title: Fixture.Create<DataString>(),
					Description: Fixture.Create<DataString>(),
					Uri: null!,
					IconName: Fixture.Create<string>())]),
			GetCollectionItemPropertyName(
				item => item.Profiles!,
				item => item.Uri),
			NotNullValidatorName);

		yield return (
			new ResumeData(
				Name: Fixture.Create<DataString>(),
				Surname: Fixture.Create<DataString>(),
				Title: Fixture.Create<DataString>(),
				Awards: [new(
					Title: invalidDataString,
					Description: Fixture.Create<DataString>())]),
			GetCollectionItemPropertyName(
				item => item.Awards!,
				item => item.Title.En),
			NotEmptyValidatorName);

		yield return (
			new ResumeData(
				Name: Fixture.Create<DataString>(),
				Surname: Fixture.Create<DataString>(),
				Title: Fixture.Create<DataString>(),
				Portfolio: [new(
					Title: Fixture.Create<DataString>(),
					Description: Fixture.Create<DataString>(),
					Uri: null!,
					ImageUri: Fixture.Create<Uri>())]),
			GetCollectionItemPropertyName(
				item => item.Portfolio!,
				item => item.Uri),
			NotNullValidatorName);

		yield return (
			new ResumeData(
				Name: Fixture.Create<DataString>(),
				Surname: Fixture.Create<DataString>(),
				Title: Fixture.Create<DataString>(),
				Clients: [new(
					Name: Fixture.Create<DataString>(),
					Uri: null!,
					Logo: Fixture.Create<Uri>())]),
			GetCollectionItemPropertyName(
				item => item.Clients!,
				item => item.Uri),
			NotNullValidatorName);
	}

	[TestMethod(ValidateWithIncorrectDataDisplayName)]
	[DynamicData(nameof(ValidateWithInvalidValueTestData))]
	public override void ValidateWithInvalidValue(
		ResumeData value,
		string propertyName,
		string expectedErrorCode)
		=> base.ValidateWithInvalidValue(value, propertyName, expectedErrorCode);
}

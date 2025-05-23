#pragma warning disable CA1515, CA1036

using Application.Infrastructure.Validation;

using FluentValidation;

namespace Application.Data.Models;

/// <summary>
/// Базовый класс для событий на временной линии. В наследованных классах должен быть
/// реализован метод для сравнения/сортировки событий.
/// </summary>
/// <param name="Institution">Организация или учреждение, в котором происходило событие.</param>
/// <param name="Position">Занимаемая позиция в организации или учреждении.</param>
/// <param name="Location">Местоположение организации или учреждении.</param>
/// <param name="Description">Описание деятельности в организации или учреждении.</param>
public abstract record TimeLineItemBase<T>(
	DataString Institution,
	DataString Position,
	DataString Location,
	DataString Description)
	: IComparable<T>
	where T : TimeLineItemBase<T>
{
	/// <inheritdoc/>
	public abstract int CompareTo(T? other);
}

internal sealed class TimeLineItemBaseValidator<T> : AbstractValidator<TimeLineItemBase<T>>
	where T : TimeLineItemBase<T>
{
	public TimeLineItemBaseValidator()
	{
		DataStringValidator dataStringValidator = new();

		this.SetDataStringRule(
			dataStringValidator,
			item => item.Institution,
			item => item.Position,
			item => item.Location,
			item => item.Description);
	}
}

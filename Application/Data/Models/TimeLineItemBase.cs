using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using Application.Infrastructure.Validation;

namespace Application.Data.Models;

/// <summary>
/// Базовый класс для событий на временной линии. В наследованных классах должен быть реализован метод для сравнения/сортировки событий.
/// </summary>
/// <param name="Institution">Организация или учреждение, в котором происходило событие.</param>
/// <param name="Position">Занимаемая позиция в организации или учреждении.</param>
/// <param name="Location">Местоположение организации или учреждении.</param>
/// <param name="Description">Описание деятельности в организации или учреждении.</param>
public abstract record TimeLineItemBase(
	[property: Required, ValidateComplexType] DataString Institution,
	[property: Required, ValidateComplexType] DataString Position,
	[property: Required, ValidateComplexType] DataString Location,
	[property: Required, ValidateComplexType] DataString Description)
	: IComparable<TimeLineItemBase>
{
	public abstract int CompareTo([AllowNull] TimeLineItemBase other);
}
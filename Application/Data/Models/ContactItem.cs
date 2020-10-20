namespace Application.Data.Models
{
	/// <summary>
	/// Модель для представления данных раздела "Контакты".
	/// </summary>
	public sealed class ContactItem : TitleElement
	{
		/// <summary>
		/// Гиперссылка контакта. Поддерживается любой тип специализированных ссылок, таких как "mailto:", "tel:" и т.п.
		/// </summary>
		public string? Hyperlink { get; set; }
	}
}

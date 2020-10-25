using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Localization.Infrastructure
{
	/// <summary>
	/// Преобразователь культуры предоставляет сведения о используемой культуре приложения и оповещает об её изменении.
	/// </summary>
	public class CultureChanger
	{
		/// <summary>
		/// Объект, представляющий язык и региональные параметры, используемые приложением.
		/// </summary>
		public static CultureInfo CurrentCulture { get; private set; } = CultureInfo.CurrentCulture;

		/// <summary>
		/// Язык и региональные параметры, используемые диспетчером ресурсов для поиска ресурсов, связанных с языком и региональными параметрами, во время выполнения.
		/// </summary>
		public static CultureInfo CurrentUICulture { get; private set; } = CultureInfo.CurrentUICulture;

		/// <summary>
		/// Оповещает об изменении культуры приложения.
		/// </summary>
		public event Func<Task>? CultureChanged;

		/// <summary>
		/// Инициирует изменение культуры приложения.
		/// </summary>
		/// <param name="cultureName">Предварительно определенное имя культуры.</param>
		public void ChangeCulture(string cultureName)
		{
			CultureInfo culture = new CultureInfo(cultureName);
			ChangeCulture(culture, culture);
		}

		/// <summary>
		/// Инициирует изменение культуры приложения.
		/// </summary>
		/// <param name="culture">Новая культура приложения. Используется для установки региональных стандартов и языка пользовательского интерфейса.</param>
		public void ChangeCulture(CultureInfo culture) => ChangeCulture(culture, culture);

		/// <summary>
		/// Инициирует изменение культуры приложения.
		/// </summary>
		/// <param name="cultureName">Предварительно определенное имя культуры, которая представляет язык и региональные параметры, используемые приложением.</param>
		/// <param name="uiCultureName">Предварительно определенное имя культуры, которая представляет язык и региональные параметры, используемые диспетчером ресурсов для поиска ресурсов, связанных с языком и региональными параметрами, во время выполнения.</param>
		public void ChangeCulture(string cultureName, string uiCultureName) => ChangeCulture(new CultureInfo(cultureName), new CultureInfo(uiCultureName));

		/// <summary>
		/// Инициирует изменение культуры приложения.
		/// </summary>
		/// <param name="culture">Получает или задает объект, который представляет язык и региональные параметры, используемые приложением.</param>
		/// <param name="uiCulture">Язык и региональные параметры, используемые диспетчером ресурсов для поиска ресурсов, связанных с языком и региональными параметрами, во время выполнения.</param>
		public void ChangeCulture(CultureInfo culture, CultureInfo uiCulture)
		{
			if (culture is null)
			{
				throw new ArgumentNullException(nameof(culture));
			}
			if (uiCulture is null)
			{
				throw new ArgumentNullException(nameof(uiCulture));
			}

			CurrentCulture = culture;
			CurrentUICulture = uiCulture;

			CultureInfo.CurrentCulture = culture;
			CultureInfo.CurrentUICulture = uiCulture;

			CultureChanged?.Invoke();
		}
	}
}

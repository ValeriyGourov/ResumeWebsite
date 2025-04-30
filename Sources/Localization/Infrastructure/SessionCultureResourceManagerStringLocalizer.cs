using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Resources;

using CommunityToolkit.Diagnostics;

using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.Infrastructure;

/// <summary>
/// Сервис, предоставляющий локализованные строки в рамках одной сессии без перезагрузки
/// приложения или обновления страницы.
/// </summary>
/// <remarks>
/// Локализованные строки возвращаются в зависимости от культуры, указанной в свойстве
/// <see cref="CultureChanger.CurrentUICulture"/>.
/// </remarks>
/// <param name="resourceManager">
/// <inheritdoc cref="_resourceManager" path="/summary"/>
/// </param>
/// <param name="logger">Средство ведения журнала.</param>
/// <exception cref="ArgumentNullException">Не указан диспетчер ресурсов.</exception>
/// <exception cref="ArgumentNullException">Не указан регистратор событий.</exception>
internal partial class SessionCultureResourceManagerStringLocalizer(
	ResourceManager resourceManager,
	ILogger logger)
	: IStringLocalizer, IDisposable
{
	private bool _disposedValue;

	/// <summary>
	/// Диспетчер ресурсов.
	/// </summary>
	private readonly ResourceManager _resourceManager = resourceManager
		?? throw new ArgumentNullException(nameof(resourceManager));

	/// <inheritdoc/>
	public LocalizedString this[string name]
	{
		get
		{
			Guard.IsNotNull(name);

			string? value = GetStringSafely(name);

			return new LocalizedString(
				name,
				value ?? name,
				resourceNotFound: value is null);
		}
	}

	/// <inheritdoc/>
	public LocalizedString this[string name, params object[] arguments]
	{
		get
		{
			Guard.IsNotNull(name);

			string? format = GetStringSafely(name);
			string? value = string.Format(CultureChanger.CurrentUICulture, format ?? name, arguments);

			return new LocalizedString(
				name,
				value,
				resourceNotFound: format is null);
		}
	}

	/// <inheritdoc/>
	public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
	{
		using ResourceSet? resources = _resourceManager.GetResourceSet(
			CultureChanger.CurrentUICulture,
			true,
			includeParentCultures);

		if (resources is not null)
		{
			foreach (DictionaryEntry item in resources)
			{
				string name = (item.Key as string)!;
				string? value = item.Value as string;

				yield return new LocalizedString(
					name,
					value ?? name,
					resourceNotFound: value is null);
			}
		}
		else
		{
			yield break;
		}
	}

	/// <summary>
	/// Выполняет попытку безопасного получения локализованной строки.
	/// </summary>
	/// <remarks>
	/// Если требуемой культуры невозможно найти файлы ресурсов, в качестве локализованной
	/// строки будет возвращено значение <see langword="null"/>.
	/// </remarks>
	/// <param name="name">Имя ресурса строки.</param>
	/// <returns>Локализованная строка.</returns>
	protected virtual string? GetStringSafely(string name)
	{
		Guard.IsNotNull(name);

		CultureInfo culture = CultureChanger.CurrentUICulture;
		string? value = null;

		while (culture != culture.Parent)
		{
			void ResourcesNotFound(SystemException exception) => LogDebugResourcesNotFound(exception);

			try
			{
				value = _resourceManager.GetString(name, culture);
			}
			catch (MissingManifestResourceException exception)
			{
				ResourcesNotFound(exception);
			}
			catch (MissingSatelliteAssemblyException exception)
			{
				ResourcesNotFound(exception);
			}

			if (value != null)
			{
				break;
			}

			culture = culture.Parent;
		}

		return value;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			if (disposing)
			{
				// Здесь необходимо освободить управляемое состояние (управляемые объекты).

				_resourceManager?.ReleaseAllResources();
			}

			// Здесь необходимо освободить неуправляемые ресурсы (неуправляемые объекты)
			// и переопределить метод завершения.
			// Здесь необходимо установить значение NULL для больших полей.
			_disposedValue = true;
		}
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		// Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	#region Методы журналирования

	[ExcludeFromCodeCoverage]
	[LoggerMessage(
		Level = LogLevel.Debug,
		Message = "Ресурсы не найдены.")]
	private partial void LogDebugResourcesNotFound(SystemException exception);

	#endregion
}

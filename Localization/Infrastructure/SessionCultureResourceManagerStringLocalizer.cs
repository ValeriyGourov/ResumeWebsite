using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("Localization.Tests")]

namespace Localization.Infrastructure
{
	/// <summary>
	/// Сервис, предоставляющий локализованные строки в рамках одной сессии без перезагрузки приложения или обновления страницы.
	/// </summary>
	/// <remarks>
	/// Локализованные строки возвращаются в зависимости от культуры, указанной в свойстве <see cref="CultureChanger.CurrentUICulture"/>.
	/// </remarks>
	internal class SessionCultureResourceManagerStringLocalizer : IStringLocalizer, IDisposable
	{
		/// <summary>
		/// Диспетчер ресурсов.
		/// </summary>
		private readonly ResourceManager _resourceManager;

		/// <summary>
		/// Регистратор событий.
		/// </summary>
		private readonly ILogger _logger;

		private bool _disposedValue;

		/// <summary>
		/// Создаёт экземпляр сервиса на основе диспетчера ресурсов.
		/// </summary>
		/// <param name="resourceManager">Диспетчер ресурсов.</param>
		/// <param name="logger">Регистратор событий.</param>
		/// <exception cref="ArgumentNullException">Не указан диспетчер ресурсов.</exception>
		/// <exception cref="ArgumentNullException">Не указан регистратор событий.</exception>
		public SessionCultureResourceManagerStringLocalizer(ResourceManager resourceManager, ILogger logger)
		{
			_resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <inheritdoc/>
		public LocalizedString this[string name]
		{
			get
			{
				if (name is null)
				{
					throw new ArgumentNullException(nameof(name));
				}

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
				if (name is null)
				{
					throw new ArgumentNullException(nameof(name));
				}

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
			ResourceSet resources = _resourceManager.GetResourceSet(
				CultureChanger.CurrentUICulture,
				true,
				includeParentCultures);

			if (resources != null)
			{
				foreach (DictionaryEntry item in resources)
				{
					string? name = item.Key as string;
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

		[Obsolete("Этот метод устарел и не делает ничего. Вместо этого используйте '" + nameof(CultureChanger.CurrentCulture) + "' и '" + nameof(CultureChanger.CurrentUICulture) + "' для '" + nameof(CultureChanger) + "'.")]
		public IStringLocalizer WithCulture(CultureInfo culture) => this;

		/// <summary>
		/// Выполняет попытку безопасного получения локализованной строки.
		/// </summary>
		/// <remarks>
		/// Если требуемой культуры невозможно найти файлы ресурсов, в качестве локализованной строки будет возвращено значение <see cref="null"/>.
		/// </remarks>
		/// <param name="name"></param>
		/// <returns></returns>
		protected virtual string? GetStringSafely(string name)
		{
			if (name is null)
			{
				throw new ArgumentNullException(nameof(name));
			}

			CultureInfo culture = CultureChanger.CurrentUICulture;
			string? value = null;

			while (culture != culture.Parent)
			{
				void ResourcesNotFound(SystemException exception) => _logger.LogDebug(exception, "Ресурсы не найдены.");

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

				// Здесь необходимо освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить метод завершения.
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
	}
}

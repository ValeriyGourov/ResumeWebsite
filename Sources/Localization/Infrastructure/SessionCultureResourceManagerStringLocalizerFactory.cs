using System.Reflection;
using System.Resources;

using CommunityToolkit.Diagnostics;

using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Localization.Infrastructure;

/// <summary>
/// Представляет фабрику, создающую экземпляры
/// <see cref="SessionCultureResourceManagerStringLocalizer"/>.
/// </summary>
/// <param name="localizationOptions">
/// <inheritdoc cref="_localizationOptions" path="/summary"/>
/// </param>
/// <param name="loggerFactory">
/// <inheritdoc cref="_loggerFactory" path="/summary"/>
/// </param>
/// <exception cref="ArgumentNullException">Не указаны параметры локализации.</exception>
/// <exception cref="ArgumentNullException">
/// Не указана фабрика регистраторов событий.
/// </exception>
internal class SessionCultureResourceManagerStringLocalizerFactory(
	IOptions<LocalizationOptions> localizationOptions,
	ILoggerFactory loggerFactory)
	: IStringLocalizerFactory
{
	/// <summary>
	/// Фабрика средств ведения журнала.
	/// </summary>
	private readonly ILoggerFactory _loggerFactory = loggerFactory
		?? throw new ArgumentNullException(nameof(loggerFactory));

	/// <summary>
	/// Параметры локализации приложения.
	/// </summary>
	private readonly IOptions<LocalizationOptions> _localizationOptions = localizationOptions
		?? throw new ArgumentNullException(nameof(localizationOptions));

	/// <inheritdoc/>
	public IStringLocalizer Create(string baseName, string location)
		=> CreateCultureResourceManagerStringLocalizer(
			baseName,
			Assembly.Load(location));

	/// <inheritdoc/>
	public IStringLocalizer Create(Type resourceSource)
	{
		Guard.IsNotNull(resourceSource);

		Assembly assembly = resourceSource.Assembly;
		string assemblyName = assembly.GetName().Name ?? string.Empty;
		string resourcesPath = _localizationOptions.Value.ResourcesPath;

		string? baseName = resourceSource.FullName;
		if (baseName != null)
		{
			int insertIndex = baseName.IndexOf(assemblyName + ".", StringComparison.OrdinalIgnoreCase) + assemblyName.Length + 1;
			baseName = baseName.Insert(insertIndex, resourcesPath + ".");
		}
		else
		{
			baseName = resourcesPath;
		}

		return CreateCultureResourceManagerStringLocalizer(baseName, assembly);
	}

	/// <summary>
	/// Создаёт экземпляр сервиса, предоставляющего локализованные строки.
	/// </summary>
	/// <param name="baseName">
	/// Корневое имя файла ресурсов без расширения, но включающее какое-либо полное имя
	/// пространства имен. К примеру, имя корневой папки для файла ресурсов
	/// MyApplication.MyResource.en-US.resources будет MyApplication.MyResource.
	/// </param>
	/// <param name="assembly">Главная сборка для ресурсов.</param>
	/// <returns>Сервис, предоставляющий локализованные строки.</returns>
	private SessionCultureResourceManagerStringLocalizer CreateCultureResourceManagerStringLocalizer(
		string baseName,
		Assembly assembly)
	{
		ResourceManager resourceManager = new(baseName, assembly);
		ILogger logger = _loggerFactory.CreateLogger<SessionCultureResourceManagerStringLocalizer>();

		return new SessionCultureResourceManagerStringLocalizer(resourceManager, logger);
	}
}

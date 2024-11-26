﻿using Oqtane.UI;
using ToSic.Cre8magic.Analytics;
using ToSic.Cre8magic.Breadcrumbs;
using ToSic.Cre8magic.Menus;
using ToSic.Cre8magic.Settings.Internal;
using ToSic.Cre8magic.Settings.Internal.Debug;
using ToSic.Cre8magic.Settings.Internal.Journal;
using ToSic.Cre8magic.Themes.Internal;
using ToSic.Cre8magic.Themes.Settings;
using ToSic.Cre8magic.Tokens;

namespace ToSic.Cre8magic.Settings;

public interface IMagicSettingsService
{
    /// <summary>
    /// Set up the settings service with the package settings, layout name and body classes.
    /// This will result in other controls and services being able to use these settings.
    /// Otherwise, the settings are just defaulted to some standard values.
    /// </summary>
    /// <param name="themePackage"></param>
    /// <param name="layoutName"></param>
    /// <returns></returns>
    IMagicSettingsService Setup(MagicThemePackage themePackage, string? layoutName);

    /// <summary>
    /// Get lightweight theme context - basically the final name, settings and journal.
    /// </summary>
    /// <param name="pageState"></param>
    /// <returns></returns>
    internal CmThemeContext GetThemeContext(PageState pageState);

    internal CmThemeContextFull GetThemeContextFull(PageState pageState);

    // TODO: MAKE INTERNAL again - temporarily public soi
    public MagicDebugSettings Debug { get; }

    internal SettingsReader<MagicAnalyticsSettings> Analytics { get; }
    internal SettingsReader<MagicThemeDesignSettings> ThemeDesign { get; }

    internal SettingsReader<MagicLanguageSettingsData> Languages { get; }

    internal SettingsReader<MagicMenuDesignSettings> MenuDesigns { get; }

    internal SettingsReader<MagicMenuSettingsData> MenuSettings { get; }
    internal List<DataWithJournal<MagicSettingsCatalog>> Catalogs { get; }
    internal SettingsReader<MagicLanguageDesignSettings> LanguageDesigns { get; }
    internal SettingsReader<MagicBreadcrumbDesignSettings> BreadcrumbDesigns { get; }
    internal SettingsReader<MagicBreadcrumbSettingsData> Breadcrumbs { get; }

    internal TokenEngine PageTokenEngine(PageState pageState);

    ///// <summary>
    ///// Figure out which settings-name can be used.
    ///// It will first try the preferred option, then the fallback, and if that doesn't exist, it will use the default name.
    ///// </summary>
    ///// <param name="preferred"></param>
    ///// <param name="fallback"></param>
    ///// <returns></returns>
    //internal (string BestName, List<string> Journal) GetBestSettingsName(string? preferred, string fallback);
    MagicDebugState DebugState(PageState pageState);


    MagicAnalyticsSettings AnalyticsSettings(string settingsName);

    internal TDebug BypassCacheInternal<TDebug>(Func<IMagicSettingsService, TDebug> func);
}
﻿using Oqtane.UI;
using ToSic.Cre8magic.Menus;
using ToSic.Cre8magic.Settings;
using ToSic.Cre8magic.Themes.Internal;

namespace ToSic.Cre8magic.Internal;

/// <summary>
/// WIP class to provide certain debug information which is internal
/// to external projects.
/// </summary>
public static class DebugExtensions
{
    public static IEnumerable<object?>? GetLogEntries(this IMagicMenuKit menuKit) =>
        menuKit?.Context?.LogRoot.Entries;

    public static List<Exception> GetExceptions(this IMagicSettingsService settingsSvc) =>
        settingsSvc.AllCatalogs.SelectMany(c => c.Journal.Exceptions).ToList();

    public static MagicThemeContext GetThemeContext(this IMagicSettingsService settingsSvc, PageState pageState) =>
        settingsSvc.GetThemeContext(pageState);

}
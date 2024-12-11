﻿using Oqtane.UI;
using ToSic.Cre8magic.Menus;
using ToSic.Cre8magic.Settings.Internal;
using ToSic.Cre8magic.Spells.Internal;
using ToSic.Cre8magic.Themes.Internal;

namespace ToSic.Cre8magic.Internal;

/// <summary>
/// WIP class to provide certain debug information which is internal
/// to external projects.
/// </summary>
public static class DebugExtensions
{
    public static IEnumerable<object?>? GetLogEntries(this IMagicMenuKit menuKit) =>
        menuKit?.WorkContext?.LogRoot.Entries;

    public static List<Exception> GetExceptions(this IMagicSpellsService spellsSvc) =>
        spellsSvc.Library.SelectMany(c => c.Journal.Exceptions).ToList();

    public static CmThemeContext GetThemeContext(this IMagicSpellsService spellsSvc, PageState pageState) =>
        spellsSvc.GetThemeContext(pageState);

}
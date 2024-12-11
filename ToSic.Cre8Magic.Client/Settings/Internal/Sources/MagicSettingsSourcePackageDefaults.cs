﻿using ToSic.Cre8magic.Settings.Internal.Journal;

namespace ToSic.Cre8magic.Settings.Internal.Sources;

/// <summary>
/// Load the package settings defaults.
///
/// Fairly trivial, but the goal is that all sources implement the same interface.
/// </summary>
public class MagicSettingsSourcePackageDefaults : IMagicSettingsSource
{
    public int Priority => -100;

    public List<DataWithJournal<MagicSpellsBook>> SpellsBooks(MagicThemePackage themePackage) =>
        themePackage == null
            ? throw new ArgumentNullException(nameof(themePackage))
            : themePackage.Defaults == null
                ? []
                : [new(themePackage.Defaults, new())];
}
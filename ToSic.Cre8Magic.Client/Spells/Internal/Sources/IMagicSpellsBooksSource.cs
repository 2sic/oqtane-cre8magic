﻿using ToSic.Cre8magic.Settings.Internal.Journal;

namespace ToSic.Cre8magic.Spells.Internal.Sources;

public interface IMagicSpellsBooksSource
{
    List<DataWithJournal<MagicSpellsBook>> SpellsBooks(MagicThemePackage themePackage);

    /// <summary>
    /// Priority, high number means higher priority
    /// </summary>
    int Priority { get; }
}
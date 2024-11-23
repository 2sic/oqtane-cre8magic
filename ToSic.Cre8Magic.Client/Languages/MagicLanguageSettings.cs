﻿using ToSic.Cre8magic.Settings;
using ToSic.Cre8magic.Settings.Internal.Debug;
using ToSic.Cre8magic.Settings.Internal.Docs;

namespace ToSic.Cre8magic.Languages;

public record MagicLanguageSettings: MagicLanguageSettingsData, ISettingsForCodeUse, IDebugSettings
{
    public MagicLanguageSettings() { }

    /// <summary>
    /// Constructor to re-hydrate from object of base class.
    /// </summary>
    internal MagicLanguageSettings(MagicLanguageSettingsData ancestor, MagicLanguageSettings? original) : base(ancestor)
    {
        if (original == null)
            return;
        
        PartName = original.PartName;
        SettingsName = original.SettingsName;
        DesignName = original.DesignName;
        DesignSettings = original.DesignSettings;
    }
    
    /// <inheritdoc/>
    public string? PartName { get; init; }

    /// <inheritdoc/>
    public string? SettingsName { get; init; }

    public string? DesignName { get; init; }

    public MagicLanguageDesignSettings? DesignSettings { get; init; }

    MagicSettingsCatalog? IDebugSettings.Catalog { get; set; }
}
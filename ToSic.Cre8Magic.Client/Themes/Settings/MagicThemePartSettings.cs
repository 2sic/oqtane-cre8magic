﻿using ToSic.Cre8magic.Settings.Internal;

namespace ToSic.Cre8magic.Client.Themes.Settings;

public record MagicThemePartSettings: ICanClone<MagicThemePartSettings>
{
    /// <summary>
    /// For json
    /// </summary>
    public MagicThemePartSettings() {}

    public MagicThemePartSettings(MagicThemePartSettings? priority, MagicThemePartSettings? fallback = default)
    {
        Show = priority?.Show ?? fallback?.Show;
        Design = priority?.Design ?? fallback?.Design;
        Configuration = priority?.Configuration ?? fallback?.Configuration;
    }

    public MagicThemePartSettings CloneWith(MagicThemePartSettings? priority, bool forceCopy = false) =>
        priority == null ? (forceCopy ? this with { } : this) : new(priority, this);



    public MagicThemePartSettings(bool show)
    {
        Show = show;
        Design = null;
        Configuration = null;
    }

    public MagicThemePartSettings(string name)
    {
        Show = true;
        Design = name;
        Configuration = name;
    }

    /// <summary>
    /// Determines if this part should be shown or not.
    ///
    /// This allows you to configure to show / not show certain bits like breadcrumbs in certain scenarios.
    /// </summary>
    public bool? Show { get; init; }

    /// <summary>
    /// Name of the design settings to look up.
    /// </summary>
    public string? Design { get; init; }

    /// <summary>
    /// Name of the settings to look up.
    /// </summary>
    public string? Configuration { get; init; }
}
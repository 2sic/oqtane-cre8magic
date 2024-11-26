﻿using System.Text.Json.Serialization;
using ToSic.Cre8magic.Settings;
using ToSic.Cre8magic.Settings.Internal;
using ToSic.Cre8magic.Settings.Internal.Debug;
using ToSic.Cre8magic.Settings.Internal.Json;
using ToSic.Cre8magic.Themes.Settings;
using static ToSic.Cre8magic.MagicConstants;

namespace ToSic.Cre8magic.Themes;

public record MagicThemeSettings: SettingsWithInherit, IHasDebugSettings, ICanClone<MagicThemeSettings>
{
    #region Constructor & Clone
    
    [PrivateApi]
    public MagicThemeSettings() { }

    [PrivateApi]
    public MagicThemeSettings(MagicThemeSettings? priority, MagicThemeSettings? fallback = default)
        : base(priority, fallback)
    {
        Logo = priority?.Logo ?? fallback?.Logo ?? Defaults.Fallback.Logo;

        // TODO: #NamedSettings
        Parts = priority?.Parts ?? fallback?.Parts ?? new();

        Design = priority?.Design ?? fallback?.Design ?? Defaults.Fallback.Design;
        Debug = priority?.Debug ?? fallback?.Debug;
    }

    [PrivateApi]
    MagicThemeSettings ICanClone<MagicThemeSettings>.CloneUnder(MagicThemeSettings? priority, bool forceCopy = false) =>
        priority == null ? (forceCopy ? this with { } : this) : new(priority, this);

    #endregion

    /// <summary>
    /// The logo to show, should be located in the assets subfolder
    /// </summary>
    public string? Logo { get; init; }

    //public int LanguagesMin { get; init; }

    /// <summary>
    /// The parts of this theme, like breadcrumb and various menu configs
    /// </summary>
    [JsonConverter(typeof(CaseInsensitiveDictionaryConverter<MagicThemePartSettings>))]
    public Dictionary<string, MagicThemePartSettings> Parts { get; init; } = new();


    public string? Design { get; init; }

    public MagicDebugSettings? Debug { get; init; }

    internal static Defaults<MagicThemeSettings> Defaults = new(new()
    {
        Logo = "unknown-logo.png",
        Design = InheritName,
    });
}
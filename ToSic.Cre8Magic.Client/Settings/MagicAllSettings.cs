﻿using System.Text.Json.Serialization;
using Oqtane.UI;
using ToSic.Cre8magic.Analytics;
using ToSic.Cre8magic.Languages.Settings;
using ToSic.Cre8magic.Settings.Debug;
using ToSic.Cre8magic.Settings.Internal;
using ToSic.Cre8magic.Themes.Internal;
using ToSic.Cre8magic.Tokens;
using ToSic.Cre8magic.Utils;
using static System.StringComparer;

namespace ToSic.Cre8magic.Settings;

/// <summary>
/// All the current "global" settings of a page, which apply to anything on the page.
/// </summary>
public record MagicAllSettings: IHasSettingsExceptions, IHasDebugSettings
{
    internal MagicAllSettings(string name, IMagicSettingsService service, MagicThemeSettings theme, TokenEngine tokens, PageState pageState)
    {
        Name = name;
        Service = service;
        Theme = theme;
        Tokens = tokens;
        PageState = pageState;
    }

    public MagicDebugState Debug => _debug ??= DebugState(Theme);
    private MagicDebugState? _debug;

    /// <summary>
    /// This is only used to detect if debugging should be active, and the setting should come from the theme itself
    /// </summary>
    MagicDebugSettings? IHasDebugSettings.Debug => Theme.Debug;


    public MagicDebugState DebugState(object? target) => Service.Debug.GetState(target, PageState.UserIsAdmin());

    internal PageState PageState { get; }

    internal TokenEngine Tokens { get; }

    public string MagicContext { get; set; } = "";

    public string Name { get; }

    [JsonIgnore] public IMagicSettingsService Service { get; }
    [JsonIgnore] internal ThemeDesigner ThemeDesigner => _themeDesigner ??= new(this);
    private ThemeDesigner? _themeDesigner;

    public MagicThemeSettings Theme { get; }

    /// <summary>
    /// Determine if we should show a specific part
    /// </summary>
    public bool Show(string name) => Theme.Parts.TryGetValue(name, out var value) && value.Show == true;

    /// <summary>
    /// Determine the name of the design configuration of a specific part
    /// </summary>
    internal string? DesignName(string name) => Theme.Parts.TryGetValue(name, out var value) ? value.Design : null;

    /// <summary>
    /// Determine the configuration name of a specific part.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    internal string? ConfigurationName(string name) => Theme.Parts.TryGetValue(name, out var value) ? value.Configuration : null;

    internal string ConfigurationNameOrDefault(string name) => ConfigurationName(name) ?? Name;

    public MagicAnalyticsSettings Analytics => _a ??= Service.Analytics.Find(ConfigurationNameOrDefault(nameof(Analytics)), Name);
    private MagicAnalyticsSettings? _a;

    public MagicThemeDesignSettings ThemeDesign => _td ??= Service.ThemeDesign.Find(Theme.Design ?? ConfigurationNameOrDefault(nameof(Theme.Design)), Name);
    private MagicThemeDesignSettings? _td;

    public MagicLanguagesSettings Languages => _l ??= Service.Languages.Find(ConfigurationNameOrDefault(nameof(Languages)), Name);
    private MagicLanguagesSettings? _l;

    public Dictionary<string, string> DebugSources { get; } = new(InvariantCultureIgnoreCase);

    public List<Exception> Exceptions => Service.Exceptions;

}
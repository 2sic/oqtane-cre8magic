﻿using System.Text.Json.Serialization;
using ToSic.Cre8magic.Analytics;
using ToSic.Cre8magic.Breadcrumbs;
using ToSic.Cre8magic.Containers;
using ToSic.Cre8magic.Internal.Json;
using ToSic.Cre8magic.Menus;
using ToSic.Cre8magic.PageContexts;
using ToSic.Cre8magic.Spells.Debug;
using ToSic.Cre8magic.Spells.Internal.Debug;
using ToSic.Cre8magic.Themes.Settings;
using static System.StringComparer;

namespace ToSic.Cre8magic.Spells;

/// <summary>
/// This is a Spells Book; a catalog of all kinds of settings.
/// It serves as a kind of database to manage all settings, which will usually be retrieved using a name. 
/// </summary>
public record MagicSpellsBook: IHasDebugSettings
{
    /// <summary>
    /// Empty Constructor so it can be created in code and Json-Deserialized
    /// </summary>
    public MagicSpellsBook() { }


    public const string SourceDefault = "Unknown";
    /// <summary>
    /// Version number when loading from JSON to verify it's what we expect
    /// </summary>
    public float Version { get; init; }

    /// <summary>
    /// Master debug settings - would override other debugs
    /// </summary>
    public MagicDebugSettings? Debug { get; init; } = new();

    /// <summary>
    /// Source of these settings / where they came from, to ensure that we can see in debug where a value was picked up from
    /// </summary>
    public string Source { get; set; } = SourceDefault;

    /// <summary>
    /// List of Themes and mainly what parts they want to explicitly configure (e.g. determine Show, and there these parts find their settings)
    /// </summary>
    [JsonConverter(typeof(CaseInsensitiveDictionaryConverter<MagicThemeSpell>))]
    public Dictionary<string, MagicThemeSpell> Themes { get; init; } = new(InvariantCultureIgnoreCase);

    [JsonConverter(typeof(CaseInsensitiveDictionaryConverter<MagicThemeBlueprint>))]
    public Dictionary<string, MagicThemeBlueprint> ThemeBlueprints { get; init; } = new(InvariantCultureIgnoreCase);


    [JsonConverter(typeof(CaseInsensitiveDictionaryConverter<MagicAnalyticsSpell>))]
    public Dictionary<string, MagicAnalyticsSpell> Analytics { get; init; } = new(InvariantCultureIgnoreCase);



    [JsonConverter(typeof(CaseInsensitiveDictionaryConverter<MagicBreadcrumbSpell>))]
    public Dictionary<string, MagicBreadcrumbSpell> Breadcrumbs { get; init; } = new(InvariantCultureIgnoreCase);

    [JsonConverter(typeof(CaseInsensitiveDictionaryConverter<MagicBreadcrumbBlueprint>))]
    public Dictionary<string, MagicBreadcrumbBlueprint> BreadcrumbBlueprints { get; init; } = new(InvariantCultureIgnoreCase);



    [JsonConverter(typeof(CaseInsensitiveDictionaryConverter<MagicContainerSpell>))]
    public Dictionary<string, MagicContainerSpell> Containers { get; init; } = new(InvariantCultureIgnoreCase);

    [JsonConverter(typeof(CaseInsensitiveDictionaryConverter<MagicContainerBlueprint>))]
    public Dictionary<string, MagicContainerBlueprint> ContainerBlueprints { get; init; } = new(InvariantCultureIgnoreCase);

    /// <summary>
    /// Language Settings
    /// </summary>
    [JsonConverter(typeof(CaseInsensitiveDictionaryConverter<MagicLanguageSpell>))]
    public Dictionary<string, MagicLanguageSpell> Languages { get; init; } = new(InvariantCultureIgnoreCase);

    /// <summary>
    /// Design definitions of languages
    /// </summary>
    [JsonConverter(typeof(CaseInsensitiveDictionaryConverter<MagicLanguageBlueprint>))]
    public Dictionary<string, MagicLanguageBlueprint> LanguageBlueprints { get; init; } = new(InvariantCultureIgnoreCase);


    /// <summary>
    /// The menu definitions
    /// </summary>
    [JsonConverter(typeof(CaseInsensitiveDictionaryConverter<MagicMenuSpell>))]
    public Dictionary<string, MagicMenuSpell> Menus { get; init; } = new(InvariantCultureIgnoreCase);

    /// <summary>
    /// The menu definitions
    /// </summary>
    [JsonConverter(typeof(CaseInsensitiveDictionaryConverter<MagicPageContextSpell>))]
    public Dictionary<string, MagicPageContextSpell> PageContexts { get; init; } = new(InvariantCultureIgnoreCase);

    /// <summary>
    /// Design definitions of the menu
    /// </summary>
    [JsonConverter(typeof(CaseInsensitiveDictionaryConverter<MagicMenuBlueprint>))]
    public Dictionary<string, MagicMenuBlueprint> MenuBlueprints { get; init; } = new(InvariantCultureIgnoreCase);

    internal static MagicSpellsBook Fallback = new()
    {
        Version = -1.0f,
        Source = "Fallback",
    };
}
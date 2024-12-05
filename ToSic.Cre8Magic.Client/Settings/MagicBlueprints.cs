﻿using System.Text.Json.Serialization;
using ToSic.Cre8magic.Settings.Internal;
using ToSic.Cre8magic.Settings.Internal.Json;
using ToSic.Cre8magic.Utils;

namespace ToSic.Cre8magic.Settings;

/// <summary>
/// Menu Design Settings
/// </summary>
public record MagicBlueprints : SettingsWithInherit, ICanClone<MagicBlueprints>
{
    [PrivateApi]
    public MagicBlueprints() { }

    internal MagicBlueprints(MagicBlueprints? priority, MagicBlueprints? fallback = default)
        : base(priority, fallback)
    {
        Parts = MergeHelper.CloneMergeDictionaries(priority?.Parts, fallback?.Parts);
    }

    MagicBlueprints ICanClone<MagicBlueprints>.CloneUnder(MagicBlueprints? priority, bool forceCopy) =>
        priority == null ? (forceCopy ? this with { } : this) : new(priority, this);

    /// <summary>
    /// Custom, named settings for classes, values etc. as you need them in your code.
    /// For things such as `ul` or `li` or `a` tags.
    /// </summary>
    [JsonConverter(typeof(CaseInsensitiveDictionaryConverter<MagicBlueprintPart>))]
    public Dictionary<string, MagicBlueprintPart> Parts { get; init; } = new(StringComparer.OrdinalIgnoreCase);

}
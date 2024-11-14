﻿using System.Text.Json.Serialization;
using ToSic.Cre8magic.Settings.Internal;

namespace ToSic.Cre8magic.Settings;

// No base interface, because for processing we need to ensure it's always a record
public abstract record SettingsWithInherit // : IInherit
{
    protected SettingsWithInherit() { }

    /// <summary>
    /// Clone support.
    /// </summary>
    protected SettingsWithInherit(SettingsWithInherit? priority, SettingsWithInherit? fallback = default)
    {
        Inherits = priority?.Inherits ?? fallback?.Inherits;
    }

    internal const string InheritsNameInJson = "@inherits";
    [JsonPropertyName(InheritsNameInJson)]
    public string? Inherits { get; init; }

}
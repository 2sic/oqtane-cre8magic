﻿using ToSic.Cre8magic.Menus;
using ToSic.Cre8magic.Settings;
using ToSic.Cre8magic.Settings.Internal;
using ToSic.Cre8magic.Utils;
using static ToSic.Cre8magic.Client.MagicConstants;
using static ToSic.Cre8magic.Settings.Json.JsonMerger;
using static ToSic.Cre8magic.Settings.SettingsWithInherit;

namespace ToSic.Cre8magic.Services.Internal;

internal class NamedSettingsReader<TPart>(
    IMagicSettingsService parent,
    Defaults<TPart> defaults,
    Func<MagicSettingsCatalog, NamedSettings<TPart>> findList,
    Func<string, Func<string, string>>? jsonProcessing = null)
    where TPart : class, new()
{
    internal TPart Find(string name, string? defaultName = null)
    {
        var names = GetConfigNamesToCheck(name, defaultName ?? name);
        var realName = names[0];
        var cached = _cache.FindInvariant(realName);
        if (cached != null) return cached;

        // Get best part; return Fallback if nothing found
        var priority = FindPart(names);
        if (priority == null) return defaults.Fallback;

        // Check if our part declares that it inherits something
        if (priority is IInherit needsMore && needsMore.Inherits.HasText())
        {
            var inheritFrom = needsMore.Inherits;
            needsMore.Inherits = null;
            priority = FindPartAndMergeIfPossible(priority, realName, inheritFrom);
        }
        else if (priority is NamedSettings<MagicMenuDesignSettings> priorityNamed 
                 && priorityNamed.TryGetValue(InheritsNameInJson, out var value))
        {
            priorityNamed.Remove(InheritsNameInJson);
            if (value.Value != null) priority = FindPartAndMergeIfPossible(priority, realName, value.Value);
        }

        if (defaults.Foundation == null) return priority;

        var merged = Merge(priority, defaults.Foundation, parent.Logger, jsonProcessing?.Invoke(realName));
        return merged!;
    }

    private TPart FindPartAndMergeIfPossible(TPart priority, string realName, string name)
    {
        var addition = FindPart(name);
        return addition == null 
            ? priority 
            : Merge(priority, addition, parent.Logger, jsonProcessing?.Invoke(realName));
    }

    private readonly NamedSettings<TPart> _cache = new();

    private static string[] GetConfigNamesToCheck(string? configuredNameOrNull, string currentName)
    {
        if (configuredNameOrNull == InheritName) configuredNameOrNull = currentName;

        return configuredNameOrNull.HasText()
            ? new[] { configuredNameOrNull, Default }.Distinct().ToArray()
            : [Default];
    }

    internal TPart? FindPart(params string[]? names)
    {
        // Make sure we have at least one name
        if (names == null || names.Length == 0) names = [Default];

        var allSourcesAndNames = names
            .Distinct()
            .Select(name => (Settings: parent.Catalog, Name: name))
            .ToList();

        foreach (var set in allSourcesAndNames)
        {
            var result = findList(set.Settings).GetInvariant(set.Name);
            if (result != null) return result;
        }

        return default;
    }


}
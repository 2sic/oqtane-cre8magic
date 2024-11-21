﻿using ToSic.Cre8magic.Settings.Internal.Docs;
using ToSic.Cre8magic.Themes.Internal;
using ToSic.Cre8magic.Themes.Settings;
using ToSic.Cre8magic.Utils;

namespace ToSic.Cre8magic.Settings.Internal;

/// <summary>
/// Helper to figure out what name we should use for the settings we retrieve.
/// Reason is that a Theme can have many parts - such as the "menu", "sidebar", etc.
/// and these in turn can have many different variations of settings.
///
/// This helper will
/// 1. First resolve any keys such as "=" to the main name
/// 2. If no name was provided, or it is blank, will use "default"
/// 3. Then check the parts list for further renames
/// </summary>
/// <param name="mainName"></param>
/// <param name="themeSettingsParts"></param>
internal class ThemePartNameResolver(string mainName, Dictionary<string, MagicThemePartSettings> themeSettingsParts)
{
    internal ThemePartNameResolver(MagicThemeContext themeCtx)
        : this(themeCtx.SettingsName, themeCtx.ThemeSettings.Parts)
    { }

    //public void GetBestNames(FindSettingsSpecs specs)
    //{
    //    var (bestSettingsName, settingsJournal) = FindBestNameAccordingToParts(specs);
    //}


    /// <summary>
    /// Generic method to check for names, since it could be run on the Settings/Configuration property or on the DesignSettings property
    /// </summary>
    internal DataWithJournal<string> FindBestNameAccordingToParts(FindSettingsSpecs specs)
    {
        // TODO: ATM JUST USES THE PART NAME
        var (initialName, journal) = PickBestSettingsName(specs.PartName, mainName);

        // Check if we have a name-remap to consider
        // If the first test fails, we try again with the prefix
        var betterName = themeSettingsParts.TryGetValue(initialName, out var part)
            ? part.GetSetting(specs.Section)
            : null;

        // If the better name wants to use the main config name ("=") then use that and exit
        if (betterName == MagicConstants.InheritName)
            return new(mainName, journal.Concat([$"switched to inherit '{mainName}'"]).ToList());

        if (betterName == null && !string.IsNullOrEmpty(specs.Prefix) && !initialName.StartsWith(specs.Prefix))
            betterName = themeSettingsParts.TryGetValue($"{specs.Prefix}{initialName}", out part)
                ? part.GetSetting(specs.Section)
                : null;

        if (!betterName.HasValue())
            return new(initialName, journal);

        return new(betterName, journal.Concat([$"updated config to '{initialName}'"]).ToList());

    }







    public DataWithJournal<(string BestSettingsName, string BestDesignName)> GetBestNames(string? possibleName, string? prefixToCheck)
    {
        var (bestSettingsName, settingsJournal) = FindBestSettingsName(possibleName, prefixToCheck);
        var (bestDesignName, designJournal) = FindBestDesignName(possibleName, prefixToCheck);
        var result = new DataWithJournal<(string, string)>((bestSettingsName, bestDesignName), settingsJournal.Concat(designJournal).ToList());
        return result;
        //return (bestSettingsName, bestDesignName, settingsJournal.Concat(designJournal).ToList());
    }


    internal (string BestName, List<string> Messages) FindBestSettingsName(string? possibleName, string? prefixToCheck) =>
        FindBestName(possibleName, prefixToCheck, MagicThemePartsExtensions.GetPartSettingsName);

    internal (string BestName, List<string> Messages) FindBestDesignName(string? possibleName, string? prefixToCheck) =>
        FindBestName(possibleName, prefixToCheck, MagicThemePartsExtensions.GetPartDesignName);

    /// <summary>
    /// Generic method to check for names, since it could be run on the Settings/Configuration property or on the DesignSettings property
    /// </summary>
    /// <param name="possibleName"></param>
    /// <param name="prefixToCheck"></param>
    /// <param name="getRenameOrNull"></param>
    /// <returns></returns>
    private (string BestName, List<string> Messages) FindBestName(string? possibleName, string? prefixToCheck, Func<Dictionary<string, MagicThemePartSettings>, string, string?> getRenameOrNull)
    {
        var (initialName, journal) = PickBestSettingsName(possibleName, mainName);

        // Check if we have a name-remap to consider
        // If the first test fails, we try again with the prefix
        var betterName = getRenameOrNull(themeSettingsParts, initialName);

        // If the better name wants to use the main config name ("=") then use that and exit
        if (betterName == MagicConstants.InheritName)
            return(mainName, journal.Concat([$"switched to inherit '{mainName}'"]).ToList());

        if (betterName == null && !string.IsNullOrEmpty(prefixToCheck) && !initialName.StartsWith(prefixToCheck))
            betterName = getRenameOrNull(themeSettingsParts,$"{prefixToCheck}{initialName}");

        if (!betterName.HasValue())
            return (initialName, journal);

        return (betterName, journal.Concat([$"updated config to '{initialName}'"]).ToList());

    }


    /// <summary>
    /// Figure out which settings-name can be used (not empty, using "=", etc.).
    /// It will first try the preferred option, then the fallback, and if that doesn't exist, it will use the default name.
    /// </summary>
    /// <param name="preferred"></param>
    /// <param name="mainName"></param>
    /// <returns></returns>
    internal static (string BestName, List<string> Journal) PickBestSettingsName(string? preferred, string mainName)
    {
        var journal = new List<string> { $"Initial Settings Name: '{preferred}'" };

        // Check if it's just a "=" symbol, which means "inherit"
        if (preferred == MagicConstants.InheritName)
        {
            preferred = mainName;
            journal.Add($"switched to inherit '{mainName}'");
        }

        // If we have a value, use that
        if (preferred.HasText())
            return (preferred, journal);

        // If we don't have a preferred name, then don't use the main name (could be "Sidebar" or something) but instead use Default
        journal.Add($"Settings Name changed to '{MagicConstants.Default}'");
        return (MagicConstants.Default, journal);
    }

}
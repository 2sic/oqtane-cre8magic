﻿using Oqtane.UI;
using ToSic.Cre8magic.Settings.Internal.Docs;
using ToSic.Cre8magic.Settings.Internal.Journal;
using ToSic.Cre8magic.Themes.Internal;
using ToSic.Cre8magic.Themes.Settings;

namespace ToSic.Cre8magic.Settings.Internal;

internal static class MagicSettingsGetSettingsPairExtensions
{
    /// <summary>
    /// Special helper to get a very common pair of settings and design settings.
    /// 
    /// Call it by providing the settings from code (can be null), and the list of settings to get from.
    /// Same for DesignSettings from code (can be null) and the list of settings to get from.
    /// 
    /// Because the return type is the main type, and the stored settings are always the pure data-type,
    /// it needs a factory to create the final object / recombine the new design settings in to the final settings.
    /// </summary>
    /// <typeparam name="TSettings">Settings type - the one used in code, with more properties than just the settings data.</typeparam>
    /// <typeparam name="TSettingsBase">The base type of the settings, just containing the data</typeparam>
    /// <typeparam name="TDesign">The type of the design settings.</typeparam>
    /// <param name="settingsSvc"></param>
    /// <param name="pageState"></param>
    /// <param name="settings"></param>
    /// <param name="settingsReader"></param>
    /// <param name="dSettings"></param>
    /// <param name="designReader"></param>
    /// <param name="menuSettingPrefix"></param>
    /// <param name="defaultPartNameForShow"></param>
    /// <param name="finalize"></param>
    /// <returns></returns>
    internal static Data3WithJournal<TSettings, MagicThemeContext, MagicThemePartSettings?> GetBestSettingsAndDesignSettings<TSettings, TSettingsBase, TDesign>(
        this IMagicSettingsService settingsSvc,
        PageState pageState,
        TSettings? settings,
        SettingsReader<TSettingsBase> settingsReader,
        TDesign? dSettings,
        SettingsReader<TDesign> designReader,
        string menuSettingPrefix,
        string defaultPartNameForShow,
        Func<TSettingsBase, TDesign, TSettings> finalize
    )
        where TSettings : TSettingsBase, ISettingsForCodeUse, new()
        where TDesign : class, new() where TSettingsBase : class, new()
    {
        // Get the Theme Context - important for checking part names
        var themeCtx = settingsSvc.GetThemeContext(pageState);

        // Find Part which contains information for these settings,
        // e.g. what to show
        var parts = themeCtx.ThemeSettings.Parts;
        var part = parts.GetValueOrDefault(settings?.PartName ?? "dummy-prevent-error")
            ?? parts.GetValueOrDefault(defaultPartNameForShow);

        // Get Settings from specified reader using the provided settings as priority to merge
        // Note that the returned data will be of the base type, not the main settings type
        var findSettings = new FindSettingsSpecs(themeCtx, settings, ThemePartSectionEnum.Settings, menuSettingPrefix);
        var (mergedSettings, journal) = settingsReader.FindAndMerge(findSettings, settings);

        // Get Design Settings from specified reader using the provided design settings as priority to merge
        findSettings = new(themeCtx, settings, ThemePartSectionEnum.Design, menuSettingPrefix);
        var (designSettings, designJournal) = designReader.FindAndMerge(findSettings, dSettings);

        // Reconstruct the expected settings type to merge in the design use the provided finalize function
        var fullSettings = finalize(mergedSettings, designSettings);

        return new(fullSettings, themeCtx, part, journal.With(designJournal));
    }

}
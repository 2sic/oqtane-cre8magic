﻿using Oqtane.Models;
using static ToSic.Cre8Magic.Client.Themes.Settings.MagicThemeDesignSettings;

namespace ToSic.Cre8Magic.Client.Containers.Settings;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// As of now, doesn't support @inherits
/// </remarks>
public class MagicContainerDesignSettings : NamedSettings<MagicContainerDesignSettingsItem>
{
    private const string ModulePrefixDefault = "module";

    internal string? Classes(MagicSettings settings, Module module, string tag)
    {
        var styles = this.FindInvariant(tag); // safe, also does null-check
        if (styles is null) return null;

        var value = new ContainerDesigner(module).GetClasses(styles);
        var tokens = settings.Tokens.Expanded(new ModuleTokens(module));
        return tokens.Parse(value);
    }

    internal static Defaults<MagicContainerDesignSettings> Defaults = new()
    {
        Fallback = new()
        {
            {
                "div", new()
                {
                    Classes = $"{MainPrefix}-page-language {SettingFromDefaults}",
                    IsPublished = new(null, $"{ModulePrefixDefault}-unpublished  {SettingFromDefaults}"),
                    IsAdminModule = new($"{MainPrefix}-admin-container  {SettingFromDefaults}"),
                }
            },
        },
    };
}
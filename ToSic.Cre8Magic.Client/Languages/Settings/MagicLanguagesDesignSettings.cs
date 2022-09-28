﻿using static ToSic.Cre8Magic.Client.Themes.Settings.MagicThemeDesignSettings;

namespace ToSic.Cre8Magic.Client.Languages.Settings;

public class MagicLanguagesDesignSettings: NamedSettings<DesignSettingActive>
{
    internal string Classes(string tag, MagicLanguage? lang = null)
    {
        if (!tag.HasValue()) return "";
        if (!this.Any()) return "";
        var styles = this.FindInvariant(tag);
        if (styles is null) return "";
        return styles.Classes + " " + styles.IsActive.Get(lang?.IsActive);
    }

    internal static Defaults<MagicLanguagesDesignSettings> Defaults = new()
    {
        Fallback = new()
        {
            { "ul", new() { Classes = $"{MainPrefix}-page-language {SettingFromDefaults}" } },
            { "li", new() { IsActive = new($"active {SettingFromDefaults}") } },
        },
    };
}
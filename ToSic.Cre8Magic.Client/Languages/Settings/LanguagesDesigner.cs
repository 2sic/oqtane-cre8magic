﻿using ToSic.Cre8magic.Utils;

namespace ToSic.Cre8magic.Languages.Settings;

internal class LanguagesDesigner(MagicSettings settings) : ThemeDesigner(settings)
{
    internal string Classes(MagicLanguage? lang, string tag)
    {
        if (!tag.HasValue()) return "";
        var styles = GetSettings(tag);
        if (styles is null) return "";
        return styles.Classes + " " + styles.IsActive.Get(lang?.IsActive);
    }

}
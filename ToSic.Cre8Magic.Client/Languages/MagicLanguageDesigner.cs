﻿using ToSic.Cre8magic.Settings;
using ToSic.Cre8magic.Themes.Internal;
using ToSic.Cre8magic.Utils;

namespace ToSic.Cre8magic.Languages;

internal class MagicLanguageDesigner(MagicAllSettings allSettings) : ThemeDesigner(allSettings)
{
    internal string Classes(MagicLanguage? lang, string tag)
    {
        if (!tag.HasValue()) return "";
        var styles = GetSettings(tag);
        if (styles is null) return "";
        return styles.Classes + " " + styles.IsActive.Get(lang?.IsActive);
    }

}
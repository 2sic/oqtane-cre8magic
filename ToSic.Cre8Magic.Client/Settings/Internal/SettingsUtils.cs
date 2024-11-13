﻿using ToSic.Cre8magic.Utils;

namespace ToSic.Cre8magic.Settings.Internal;

internal static class SettingsUtils
{
    public static string RandomLongId() => new Random().Next(100000, 1000000).ToString();

    public static string RandomLongId(string? id) => id.HasText()
        ? id
        : RandomLongId();
}
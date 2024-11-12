﻿namespace ToSic.Cre8magic.Settings.Debug;

internal interface IHasDebugSettings
{
    /// <summary>
    /// Debug settings for anything that can configure show/hide of debug
    /// </summary>
    MagicDebugSettings? Debug { get; }
}
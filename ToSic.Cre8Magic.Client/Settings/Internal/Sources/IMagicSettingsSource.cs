﻿namespace ToSic.Cre8magic.Settings.Internal.Sources;

public interface IMagicSettingsSource
{
    SettingsSourceInfo? Get(MagicPackageSettings packageSettings);

    /// <summary>
    /// Priority, high number means higher priority
    /// </summary>
    int Priority { get; }
}
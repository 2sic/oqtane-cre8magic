﻿using ToSic.Cre8magic.Settings;
using ToSic.Cre8magic.Settings.Internal;

namespace ToSic.Cre8magic.Containers;

public record MagicContainerSettings: MagicSettingsBase, ICanClone<MagicContainerSettings>
{
    public MagicContainerSettings() { }

    [PrivateApi]
    public MagicContainerSettings(MagicContainerSettings? priority, MagicContainerSettings? fallback = default)
        : base(priority, fallback)
    {
        DesignSettings = priority?.DesignSettings ?? fallback?.DesignSettings;
    }

    [PrivateApi]
    public MagicContainerSettings CloneUnder(MagicContainerSettings? priority, bool forceCopy = false) =>
        priority == null ? (forceCopy ? this with { } : this) : new(priority, this);

    public MagicContainerDesignSettings? DesignSettings { get; init; }

    internal static Defaults<MagicContainerSettings> Defaults = new(new());

}
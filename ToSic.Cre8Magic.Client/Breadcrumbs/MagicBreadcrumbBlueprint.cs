﻿using ToSic.Cre8magic.Spells.Internal;
using ToSic.Cre8magic.Tailors;

namespace ToSic.Cre8magic.Breadcrumbs;

/// <summary>
/// Language Design Settings
/// </summary>
public record MagicBreadcrumbBlueprint : MagicBlueprint, ICanClone<MagicBreadcrumbBlueprint>
{
    [PrivateApi]
    public MagicBreadcrumbBlueprint() { }

    private MagicBreadcrumbBlueprint(MagicBreadcrumbBlueprint? priority, MagicBreadcrumbBlueprint? fallback = default)
        : base(priority, fallback)
    { }

    MagicBreadcrumbBlueprint ICanClone<MagicBreadcrumbBlueprint>.CloneUnder(MagicBreadcrumbBlueprint? priority, bool forceCopy) =>
        priority == null ? (forceCopy ? this with { } : this) : new(priority, this);


    internal static Defaults<MagicBreadcrumbBlueprint> DesignDefaults = new()
    {
        Fallback = new()
        {
            Parts = new()
            {
                { "li", new() { IsActive = new() { On = "active" } } }
            },
        },
        Foundation = new()
    };
}
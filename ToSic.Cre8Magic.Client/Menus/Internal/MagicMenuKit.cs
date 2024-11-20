﻿using ToSic.Cre8magic.Pages;
using ToSic.Cre8magic.Settings;

namespace ToSic.Cre8magic.Menus.Internal;

/// <summary>
/// tbd
/// </summary>
public record MagicMenuKit : IMagicMenuKit
{
    public required IMagicPageList Pages { get; init; }

    public required IMagicPageSetSettings Settings { get; init; }

    // TODO:
    //public object Designer => Pages.Settings.Designer;

    public string Variant => Settings.Variant ?? "";

    public /* actually internal */ required IContextWip Context { get; init; }

    // TODO: naming not final
    public IMagicMenuKit Kit(IMagicPageWithDesignWip page) =>
        this with { Pages = page };
}
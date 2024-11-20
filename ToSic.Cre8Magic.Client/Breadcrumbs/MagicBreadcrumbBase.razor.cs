﻿using Microsoft.AspNetCore.Components;
using Oqtane.UI;
using ToSic.Cre8magic.Components.Internal;
using ToSic.Cre8magic.Pages;
using ToSic.Cre8magic.Utils;

namespace ToSic.Cre8magic.Breadcrumb;

/// <summary>
/// Recommended base class for all breadcrumb components.
/// </summary>
public abstract class MagicBreadcrumbBase: ComponentBase
{
    [Inject]
    public IMagicPageService? PageServiceWip { get; set; }

    [Inject]
    public required IMagicBreadcrumbService BreadcrumbService { get; set; }

    /// <inheritdoc cref="ComponentDocs.PageState"/>
    [CascadingParameter]
    public required PageState PageState { get; set; }

    /// <summary>
    /// Settings for retrieving the breadcrumbs; optional.
    /// If not set, the current page will be used as the active page.
    /// </summary>
    [Parameter]
    public MagicBreadcrumbSettings? Settings { get; set; }


    /// <summary>
    /// WIP experimental pattern. Probably not the best/final implementation yet...
    /// </summary>
    /// <param name="settings"></param>
    /// <returns></returns>
    protected virtual MagicBreadcrumbSettings Customize(MagicBreadcrumbSettings settings)
        => settings;

    // TODO: move to kit, or consider removing
    // TODO: note also that we're using BreadcrumbKit.Pages.Classes(...) somewhere, so we should add the designer to the kit
    // The home page - never changes during runtime, so we can cache it
    protected IMagicPage HomePage => _homePage ??= PageServiceWip!.GetHome(PageState);
    private IMagicPage? _homePage;

    /// <summary>
    /// The Breadcrumb for the current page.
    /// Will be updated when the page changes.
    /// </summary>
    protected IMagicBreadcrumbKit BreadcrumbKit => _breadcrumbKit.Get(PageState, () => BreadcrumbService.BreadcrumbKit(PageState, Customize(Settings ?? new())));
    private readonly GetKeepByPageId<IMagicBreadcrumbKit> _breadcrumbKit = new();

}
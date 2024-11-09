﻿using ToSic.Cre8magic.Pages;

namespace ToSic.Cre8magic.Client.Breadcrumbs;

public abstract class MagicBreadcrumbs: MagicControl
{
    // The home page - never changes during runtime, so we can cache it
    protected IMagicPage HomePage => _homePage ??= PageFactory.Home;
    private IMagicPage? _homePage;

    protected List<IMagicPage> Breadcrumbs
    {
        get
        {
            // Reset cache if the page has changed
            if (_lastPageId != PageState.Page.PageId) _breadcrumbs = null;
            _lastPageId = PageState.Page.PageId;
            // Return cached or new breadcrumbs
            return _breadcrumbs ??= PageFactory.Breadcrumbs(); // PageState.Breadcrumbs();
        }
    }

    private int? _lastPageId;
    private List<IMagicPage>? _breadcrumbs;
}
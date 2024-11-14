﻿using Oqtane.UI;
using ToSic.Cre8magic.Pages;

namespace ToSic.Cre8magic.Menus;

/// <summary>
/// Service to get a tree of pages for a menu.
/// </summary>
public interface IMagicMenuService
{
    public bool NoInheritSettingsWip { get; set; }

    /// <summary>
    /// Get the menu items for the current page and specified settings.
    /// </summary>
    /// <param name="pageState"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    IMagicPageList GetMenu(PageState pageState, MagicMenuSettings? settings = default);
}
﻿using Oqtane.Models;
using Oqtane.UI;
using ToSic.Cre8magic.Analytics;
using ToSic.Cre8magic.Breadcrumbs;
using ToSic.Cre8magic.Breadcrumbs.Internal;
using ToSic.Cre8magic.Containers;
using ToSic.Cre8magic.Languages.Internal;
using ToSic.Cre8magic.Links;
using ToSic.Cre8magic.PageContexts;
using ToSic.Cre8magic.PageContexts.Internal;
using ToSic.Cre8magic.Settings;
using ToSic.Cre8magic.Settings.Internal.Providers;
using ToSic.Cre8magic.Themes.Internal;
using ToSic.Cre8magic.UserLogins.Internal;
using ToSic.Cre8magic.Users;

namespace ToSic.Cre8magic.Internal;

internal class MagicHat(
    MagicLazy<IMagicAnalyticsService> analyticsSvc,
    MagicLazy<IMagicBreadcrumbService> breadcrumbSvc,
    IMagicSettingsService settingsSvc,
    MagicLazy<IMagicLanguageService> languageSvc,
    MagicLazy<IMagicPageContextService> pageContextSvc,
    MagicLazy<IMagicUserService> userSvc,
    MagicLazy<IUserLoginService> userKitSvc,
    MagicLazy<IMagicThemeService> themeSvc,
    MagicLazy<IMagicSettingsProvider> settingsProviderSvc,
    MagicLazy<IMagicLinkService> linkSvc) : IMagicHat
{
    /// <inheritdoc />
    public IMagicAnalyticsKit AnalyticsKit(PageState pageState, MagicAnalyticsSettings? settings = default) =>
        analyticsSvc.Value.AnalyticsKit(pageState, settings);

    /// <inheritdoc />
    public IMagicBreadcrumbKit BreadcrumbKit(PageState pageState, MagicBreadcrumbSettings? settings = null) =>
        breadcrumbSvc.Value.BreadcrumbKit(pageState, settings);

    /// <inheritdoc />
    public Task<IMagicLanguageKit> LanguageKitAsync(PageState pageState, MagicLanguageSettings? settings = default) =>
        languageSvc.Value.LanguageKitAsync(pageState, settings);


    /// <inheritdoc />
    public IMagicPageContextKit PageContextKit(PageState pageState, MagicPageContextSettings? settings = default) =>
        pageContextSvc.Value.PageContextKit(pageState, settings);

    /// <inheritdoc />
    public MagicUser User(PageState pageState) =>
        userSvc.Value.User(pageState);

    public IMagicUserLoginKit UserLoginKit(PageState pageState) =>
        userKitSvc.Value.UserLoginKit(pageState);

    public IMagicContainerKit ContainerKit(PageState pageState, Module module)
    {
        var designer = ContainerDesigner(pageState, module);
        return new MagicContainerKit
        {
            Designer = designer,
            Module = module
        };
    }

    public string Link(PageState pageState, MagicLinkSpecs linkSpecs) =>
        linkSvc.Value.Link(pageState, linkSpecs);

    /// <inheritdoc />
    public IMagicThemeKit ThemeKit(PageState pageState) =>
        themeSvc.Value.ThemeKit(pageState);

    public void UseSettingsPackage(MagicThemePackage themePackage, string? layoutName = default)
    {
        settingsSvc.Setup(themePackage, layoutName);
    }

    public void UseSettingsCatalog(MagicSettingsCatalog catalog)
    {
        settingsProviderSvc.Value.Provide(catalog);
    }

    public void UseSettingsProvider(Func<IMagicSettingsProvider, IMagicSettingsProvider> providerFunc)
    {
        var provider = new MagicSettingsProvider();
        var result = providerFunc(provider);
        var cat = result?.Catalog;
        if (cat != null)
            settingsProviderSvc.Value.Provide(cat);
    }


    private MagicContainerDesigner ContainerDesigner(PageState pageState, Module module)
    {
        if (_containerDesigners.TryGetValue(pageState.Page.PageId, out var designer))
            return designer;

        var designContext = settingsSvc.GetThemeContextFull(pageState);
        var container = new MagicContainerDesigner(designContext, module);
        _containerDesigners[module.ModuleId] = container;
        return container;
    }
    private readonly Dictionary<int, MagicContainerDesigner> _containerDesigners = new();

    public MagicThemeDesigner ThemeDesigner(PageState pageState)
    {
        if (_themeDesigners.TryGetValue(pageState.Page.PageId, out var designer))
            return designer;

        var designContext = settingsSvc.GetThemeContextFull(pageState);
        var theme = new MagicThemeDesigner(designContext);
        _themeDesigners[pageState.Page.PageId] = theme;
        return theme;
    }
    private readonly Dictionary<int, MagicThemeDesigner> _themeDesigners = new();

}
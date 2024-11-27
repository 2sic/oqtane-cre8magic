﻿using System.Runtime.CompilerServices;
using Oqtane.Models;
using Oqtane.UI;
using ToSic.Cre8magic.Analytics;
using ToSic.Cre8magic.Analytics.Internal;
using ToSic.Cre8magic.Breadcrumbs;
using ToSic.Cre8magic.Breadcrumbs.Internal;
using ToSic.Cre8magic.Containers;
using ToSic.Cre8magic.Containers.Internal;
using ToSic.Cre8magic.Languages.Internal;
using ToSic.Cre8magic.Links;
using ToSic.Cre8magic.PageContexts;
using ToSic.Cre8magic.PageContexts.Internal;
using ToSic.Cre8magic.Settings;
using ToSic.Cre8magic.Settings.Internal.Providers;
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
    MagicLazy<IMagicLinkService> linkSvc,
    MagicLazy<IMagicContainerService> containerSvc) : IMagicHat
{
    #region Setup & PageState

    public IMagicHat UseSettingsPackage(MagicThemePackage themePackage, string? layoutName = null)
    {
        settingsSvc.Setup(themePackage, layoutName);
        return this;
    }

    public IMagicHat UseSettingsCatalog(MagicSettingsCatalog catalog)
    {
        settingsProviderSvc.Value.Provide(catalog);
        return this;
    }

    public IMagicHat UseSettingsProvider(Func<IMagicSettingsProvider, IMagicSettingsProvider> providerFunc)
    {
        var provider = new MagicSettingsProvider();
        var result = providerFunc(provider);
        var cat = result?.Catalog;
        if (cat != null)
            settingsProviderSvc.Value.Provide(cat);
        return this;
    }

    public IMagicHat UsePageState(PageState pageState)
    {
        settingsSvc.UsePageState(pageState);
        return this;
    }

    private PageState GetPageStateOrThrow(PageState? pageStateFromSettings, [CallerMemberName] string? methodName = default)
    {
        var pageState = pageStateFromSettings ?? settingsSvc.PageState;
        if (pageState == null)
            throw new ArgumentException($"PageState is required for {methodName}(...). You must either supply it in the settings, or first initialize the MagicHat using {nameof(UsePageState)}(...)");
        return pageState;
    }

    #endregion

    /// <inheritdoc />
    public IMagicAnalyticsKit AnalyticsKit(MagicAnalyticsSettings? settings = null) =>
        analyticsSvc.Value.AnalyticsKit(GetPageStateOrThrow(settings?.PageState), settings);

    /// <inheritdoc />
    public IMagicBreadcrumbKit BreadcrumbKit(MagicBreadcrumbSettings? settings = null) =>
        breadcrumbSvc.Value.BreadcrumbKit(GetPageStateOrThrow(settings?.PageState), settings);

    /// <inheritdoc />
    public Task<IMagicLanguageKit> LanguageKitAsync(MagicLanguageSettings? settings = null) =>
        languageSvc.Value.LanguageKitAsync(GetPageStateOrThrow(settings?.PageState), settings);


    /// <inheritdoc />
    public IMagicPageContextKit PageContextKit(MagicPageContextSettings? settings = null) =>
        pageContextSvc.Value.PageContextKit(GetPageStateOrThrow(settings?.PageState), settings);

    /// <inheritdoc />
    public MagicUser User(PageState pageState) =>
        userSvc.Value.User(pageState);

    public IMagicUserLoginKit UserLoginKit(PageState pageState) =>
        userKitSvc.Value.UserLoginKit(pageState);

    public IMagicContainerKit ContainerKit(MagicContainerSettings settings) =>
        containerSvc.Value.ContainerKit(
            GetPageStateOrThrow(settings?.PageState),
            settings?.ModuleState ?? throw new ArgumentException($"{nameof(settings.ModuleState)} is required for {nameof(ContainerKit)}(...)")
        );

    public string Link(PageState pageState, MagicLinkSpecs linkSpecs) =>
        linkSvc.Value.Link(pageState, linkSpecs);

    /// <inheritdoc />
    public IMagicThemeKit ThemeKit(PageState pageState) =>
        themeSvc.Value.ThemeKit(pageState);
}
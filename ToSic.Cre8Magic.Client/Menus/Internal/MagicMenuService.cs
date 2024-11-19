﻿using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Oqtane.UI;
using ToSic.Cre8magic.Pages;
using ToSic.Cre8magic.Pages.Internal;
using ToSic.Cre8magic.Settings;
using ToSic.Cre8magic.Settings.Internal;
using ToSic.Cre8magic.Themes.Settings;
using ToSic.Cre8magic.Utils.Logging;

namespace ToSic.Cre8magic.Menus.Internal;

/// <summary>
/// Will create a MenuTree based on the current pages information and configuration
/// </summary>
public class MagicMenuService(ILogger<MagicMenuService> logger, IMagicSettingsService settingsSvc): IMagicMenuService
{
    public ILogger Logger { get; } = logger;

    private const string MenuSettingPrefix = "menu-";

    public bool NoInheritSettingsWip { get; set; } = false;

    public IMagicPageList GetMenu(PageState pageState, MagicMenuSettings? settings = default)
    {

        var (newSettings, messages) = NoInheritSettingsWip
        // todo: magicMenuSettings.Default.Fallback
            ? (settings ?? new(), new List<string>())
            : MergeSettings(pageState, settings);

        // Transfer Logs from Tree creation to the current log
        var logRoot = new LogRoot();
        if (messages.Any())
        {
            var messageLog = logRoot.GetLog("tree-build");
            foreach (var m in messages) messageLog.A(m);
        }


        // Add break-point for debugging during development
        if (pageState.IsDebug()) pageState.DoNothing();

        var pageFactory = new MagicPageFactory(pageState, newSettings.Pages, logRoot: logRoot);
        var pageTokens = settingsSvc.PageTokenEngine(pageState);
        var context = new ContextWip<MagicMenuSettings, IMagicPageDesigner>(
            newSettings,
            newSettings.Designer,
            pageFactory,
            pageTokens,
            logRoot: logRoot
        );

        var rootBuilder = new MagicMenuBuilder(context);
        var list = new MagicPageList(context, pageFactory, rootBuilder.NodeFactory, rootBuilder.GetChildren());
        return list;
    }

    private (MagicMenuSettings Settings, List<string> Messages) MergeSettings(PageState pageState, MagicMenuSettings? settings = default)
    {
        var themeCtx = settingsSvc.GetThemeContext(pageState);

        var nameResolver = new ThemePartNameResolver(themeCtx);
        var (settingsName, designName, journal) = nameResolver.GetMostRelevantNames(settings?.PartName, MenuSettingPrefix);


        // If the user didn't specify a config name in the Parameters or the config name
        // isn't contained in the json file the normal parameter are given to the service
        var mergedSettings = settingsSvc.MenuSettings.FindAndMerge(settingsName, themeCtx.SettingsName, settings);

        // Usually there is no Design-object pre-filled, in which case we should
        // 1. try to find it in json
        // 2. use the one from the configuration
        if (mergedSettings.DesignSettings == null)
        {
            // Check various places where design could be configured by priority
            var designSettings = settingsSvc.MenuDesigns.FindAndNeutralize(designName, themeCtx.SettingsName);
            mergedSettings = mergedSettings with { DesignSettings = designSettings };
        }
        else
            journal.Add("Design rules already set");

        return (mergedSettings, journal);
    }

}

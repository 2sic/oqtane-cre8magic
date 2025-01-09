﻿using Oqtane.UI;
using ToSic.Cre8magic.Internal.Journal;
using ToSic.Cre8magic.Pages.Internal;
using ToSic.Cre8magic.Spells.Internal;
using ToSic.Cre8magic.Utils.Logging;

namespace ToSic.Cre8magic.Menus.Internal;

/// <summary>
/// Will create a MenuTree based on the current pages information and settings.
/// </summary>
public class MagicMenuService(IMagicSpellsService spellsSvc): IMagicMenuService
{
    private const string DefaultPartName = "Menus";

    public bool NoInheritSettingsWip { get; set; } = false;

    public IMagicMenuKit MenuKit(PageState pageState, MagicMenuSpell? settings = null)
    {
        var (newSettings, journal) = NoInheritSettingsWip
            ? new(settings ?? new(), new())    // todo: magicMenuSettings.Default.Fallback
            : MergeSettings(pageState, settings);

        // Transfer Logs from Tree creation to the current log
        var logRoot = new LogRoot();
        logRoot.Add("tree-build", journal.Messages);

        // Page Factory with possibly reduced set of possible pages it can return
        var pageFactory = new MagicPageFactory(pageState, newSettings.PagesSource, logRoot: logRoot);

        // Create the Menu Context which is used in various places
        var context = new MagicMenuWorkContext
        {
            Tailor = newSettings.Tailor,
            LogRoot = logRoot,
            PageFactory = pageFactory,
            TokenEngine = spellsSvc.PageTokenEngine(pageState),
            Settings = newSettings,
        };

        var nodeFactory = new MagicMenuNodeFactory(context);
        var root = new MagicPage(new() /* fake page, just for providing classes / values to the root outside the menu */, pageFactory, nodeFactory)
        {
            IsVirtualRoot = true,
            MenuLevel = 0,
        };
        var kit = new MagicMenuKit
        {
            Root = root,
            Spell = newSettings,
            WorkContext = context,
        };
        return kit;
    }

    private DataWithJournal<MagicMenuSpell> MergeSettings(PageState pageState, MagicMenuSpell? settings) =>
        spellsSvc.GetBestSpellAndBlueprints(
            pageState,
            settings,
            spellsSvc.Menus,
            settings?.Blueprint,
            spellsSvc.MenuBlueprints, 
            DefaultPartName,
            finalize: (settingsData, designSettings) => settingsData with /* TODO: USE WITH<...> */ { Blueprint = designSettings });



}

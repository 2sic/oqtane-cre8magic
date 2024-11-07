﻿using Oqtane.UI;
using ToSic.Cre8magic.Client.Breadcrumbs;
using ToSic.Cre8magic.Client.Models;
using ToSic.Cre8magic.Client.Pages;

namespace ToSic.Cre8magic.Client.Menus;

public class MagicMenuTree : MagicMenuPage
{
    public const char PageForced = '!';

    public MagicMenuTree(PageState pageState) : this(new MagicPageFactory(pageState))
    { }

    public MagicMenuTree(MagicPageFactory pageFactory) : base(pageFactory, new MagicMenuPageHelper(pageFactory), pageFactory.Current, 1)
    {
        Log = LogRoot.GetLog("Root");
        Log.A($"Start with PageState for Page:{PageId}; Level:1");

        // update dependent properties
        MenuPages = PageFactory.MenuPages;
        Settings = MagicMenuSettings.Defaults.Fallback;
        // TODO: SOMETHING IS BUGGY HERE
        // This code works, because it sets the designer with default settings
        // but the main menu won't show if we use the configured settings
        // WIP
        Design = new MenuDesigner(/*this,*/ Settings);
        Debug = [];
    }

    internal MagicMenuTree(MagicSettings magicSettings, MagicMenuSettings settings, IEnumerable<MagicPage>? menuPages = null, List<string>? messages = null) : this(magicSettings.PageState)
    {
        Log.A($"Start with MagicSettings for Page:{PageId}; Level:1");

        Helper.Set(magicSettings);
        ((MagicMenuPageHelper)Helper).Set(settings);
        //MagicSettings = magicSettings;
        Settings = settings;
        //Design = new MenuDesigner(Settings);
        if (menuPages != null) SetMenuPages(menuPages);
        if (messages != null) SetMessages(messages);
    }

    #region Init

    public MagicMenuTree Setup(MagicMenuSettings? settings)
    {
        Log.A($"Init MagicMenuSettings Start:{settings?.Start}; Level:{settings?.Level}");
        if (settings != null)
        {
            ((MagicMenuPageHelper)Helper).Set(settings);
            Settings = settings;
        }
        return this;
    }

    public MagicMenuTree SetMenuPages(IEnumerable<MagicPage> menuPages)
    {
        Log.A($"Init menuPages:{menuPages.Count()}");
        MenuPages = menuPages;
        return this;
    }

    public MagicMenuTree SetMessages(List<string> messages)
    {
        Log.A($"Init messages:{messages.Count}");
        Debug = messages;
        return this;
    }

    public MagicMenuTree Designer(IPageDesigner pageDesigner)
    {
        Log.A($"Init MenuDesigner:{pageDesigner != null}");
        Helper.Set(pageDesigner);
        Design = pageDesigner;
        return this;
    }

    #endregion


    public MagicMenuSettings Settings { get; private set; }

    //private MagicSettings? MagicSettings { get; set; } // TODO: stv move this to better place because it is MagicSettings part

    //internal TokenEngine PageTokenEngine(MagicPage page) // TODO: stv move this to better place because it is MagicSettings part
    //{
    //    // fallback without MagicSettings return just TokenEngine with PageTokens
    //    if (MagicSettings == null)
    //        return new TokenEngine([new PageTokens(PageFactory, page)]);

    //    var originalPage = (PageTokens)MagicSettings.Tokens.Parsers.First(p => p.NameId == PageTokens.NameIdConstant);
    //    originalPage = originalPage.Clone(page, menuId: MenuId);
    //    return MagicSettings.Tokens.SwapParser(originalPage);
    //}

    /// <summary>
    /// Pages in the menu according to Oqtane pre-processing
    /// Should be limited to pages which should be in the menu, visible and permissions ok. 
    /// </summary>
    internal IEnumerable<MagicPage> MenuPages { get; private set; }

    internal override MagicMenuTree Tree => this;

    internal IPageDesigner Design { get; private set; }

    internal List<MagicPage> Breadcrumb => _breadcrumb ??= PageFactory.Breadcrumbs(this).ToList();
    private List<MagicPage>? _breadcrumb;

    public override string MenuId => _menuId ??= Settings?.MenuId ?? "error-menu-id";
    private string? _menuId;

    public int Depth => _depth ??= Settings?.Depth ?? MagicMenuSettings.LevelDepthFallback;
    private int? _depth;

    public List<string> Debug { get; private set; }

    internal LogRoot LogRoot { get; } = new();

    protected override List<MagicPage> GetChildPages() => GetRootPages();

    protected List<MagicPage> GetRootPages()
    {
        var l = Log.Fn<List<MagicPage>>($"{PageId}");
        // Give empty list if we shouldn't display it
        if (Settings.Display == false)
            return l.Return([], "Display == false, don't show");

        // Case 1: StartPage *, so all top-level entries
        var start = Settings.Start?.Trim();

        // Case 2: '.' - not yet tested
        var startLevel = Settings.Level ?? MagicMenuSettings.StartLevelFallback;
        var getChildren = Settings.Children ?? MagicMenuSettings.ChildrenFallback;
        var startingPoints = GetStartNodeRules(start, startLevel, getChildren);
        // Case 3: one or more IDs to start from

        var startPages = FindStartPageOfManyRules(startingPoints);

        return l.Return(startPages, LogPageList(startPages));
    }

    internal List<MagicPage> FindStartPageOfManyRules(StartNodeRule[] startingPoints)
    {
        var l = Log.Fn<List<MagicPage>>(string.Join(',', startingPoints.Select(p => p.Id)));
        var result = startingPoints.SelectMany(FindStartPagesOfOneRule)
            .Where(p => p != null)
            .ToList();
        return l.Return(result, LogPageList(result));
    }

    private List<MagicPage> FindStartPagesOfOneRule(StartNodeRule n)
    {
        var l = Log.Fn<List<MagicPage>>($"Include hidden pages: {n.Force}; Mode: {n.ModeInfo}");

        // Start by getting all the anchors - the pages to start from, before we know about children or not
        // Three cases: root, current, ...
        var anchorPages = FindInitialAnchorPages(n);

        var result = n.ShowChildren
            ? anchorPages.SelectMany(p => GetRelatedPagesByLevel(p, 1)).ToList()
            : anchorPages;

        return l.Return(result, LogPageList(result));
    }

    private List<MagicPage> FindInitialAnchorPages(StartNodeRule n)
    {
        var l = Log.Fn<List<MagicPage>>();
        var source = n.Force ? PageFactory.All().ToList() : Tree.MenuPages.ToList();
        switch (n.ModeInfo)
        {
            case StartMode.PageId:
                return l.Return(source.Where(p => p.PageId == n.Id).ToList(), $"Page with id {n.Id}");
            case StartMode.Root:
                return l.Return(source.Where(p => p.Level == 0).ToList(), "Home/root");
            // Level 0 means current level / current page
            case StartMode.Current when n.Level == 0:
                return l.Return([this], "Current page");
            // Level 1 means top-level pages. If we don't want the level1 children, we want the top-level items
            // TODO: CHECK WHAT LEVEL Oqtane actually gives us, is 1 the top?
            case StartMode.Current when n.Level == 1 && !n.ShowChildren:
                return l.Return(source.Where(p => p.Level == 0).ToList(), "Current level 1?");
            case StartMode.Current when n.Level > 0:
                // If coming from the top, level 1 means top level, so skip one less
                var skipDown = n.Level - 1;
                var fromTop = PageFactory.Breadcrumbs(source, this) /*  source.Breadcrumbs(this) */.Skip(skipDown).FirstOrDefault();
                var fromTopResult = fromTop == null ? [] : new List<MagicPage> { fromTop };
                return l.Return(fromTopResult, $"from root to breadcrumb by {skipDown}");
            case StartMode.Current when n.Level < 0:
                // If going up, must change skip to normal
                var skipUp = Math.Abs(n.Level);
                var fromCurrent = PageFactory.GetAncestors(source, this) /* source.GetAncestors(this) */.ToList().Skip(skipUp).FirstOrDefault();
                var result = fromCurrent == null ? [] : new List<MagicPage> { fromCurrent };
                return l.Return(result, $"up the ancestors by {skipUp}");
            default:
                return l.Return([], "nothing");
        }
    }


    private List<MagicPage> GetRelatedPagesByLevel(MagicPage referencePage, int level)
    {
        var l = Log.Fn<List<MagicPage>>($"{referencePage.PageId}; {level}");
        List<MagicPage> result;
        switch (level)
        {
            case -1:
                result = ChildrenOf(referencePage.ParentId ?? 0);
                return l.Return(result, "siblings - " + LogPageList(result));
            case 0:
                result = [referencePage];
                return l.Return(result, "current page - " + LogPageList(result));
            case 1:
                result = ChildrenOf(referencePage.PageId);
                return l.Return(result, "children - " + LogPageList(result));
            case > 1:
                return l.Return([ErrPage(0, "Error: Create menu from current page but level > 1")], "err");
            default:
                return l.Return(
                    [ErrPage(0, "Error: Create menu from current page but level < -1, not yet implemented")], "err");
        }
    }

    private StartNodeRule[] GetStartNodeRules(string? value, int level, bool showChildren)
    {
        var l = Log.Fn<StartNodeRule[]>($"{nameof(value)}: '{value}'; {nameof(level)}: {level}; {nameof(showChildren)}: {showChildren}");

        if (!value.HasText()) return l.Return([], "no value, empty list");
        var parts = value.Split(',');
        var result = parts
            .Select(fromNode =>
            {
                fromNode = fromNode.Trim();
                if (!fromNode.HasText()) return null;
                var important = fromNode.EndsWith(PageForced);
                if (important) fromNode = fromNode.TrimEnd(PageForced);
                fromNode = fromNode.Trim();
                int.TryParse(fromNode, out var id);
                return new StartNodeRule { Id = id, From = fromNode, Force = important, ShowChildren = showChildren, Level = level };
            })
            .Where(n => n != null)
            .ToArray() as StartNodeRule[];
        return l.ReturnAndKeepData(result, result.Length.ToString());
    }
}
﻿using Oqtane.UI;
using ToSic.Cre8magic.Client.Models;
using ToSic.Cre8magic.Client.Pages;

namespace ToSic.Cre8magic.Client.Menus;

public class MagicMenuTree : MagicMenuPage, IMagicPageList
{
    internal MagicMenuTree(MagicSettings magicSettings, MagicMenuSettings settings, IEnumerable<MagicPage>? menuPages = null, List<string>? messages = null)
        : this(magicSettings.PageState)
    {
        SetHelper.Set(magicSettings);
        ((MagicMenuPageSetHelper)SetHelper).Set(settings);
        if (menuPages != null) SetMenuPages(menuPages);
        if (messages != null) SetMessages(messages);
    }

    public MagicMenuTree(PageState pageState) : this(new MagicPageFactory(pageState))
    { }

    private MagicMenuTree(MagicPageFactory pageFactory) : base(pageFactory, new MagicMenuPageSetHelper(pageFactory), pageFactory.Current, 1, debugPrefix: "Root")
    {
        //PageFactory = pageFactory;
        //SetHelper = new MagicMenuPageSetHelper(pageFactory);
        //Log = SetHelper.LogRoot.GetLog("Root");
        //Settings = SetHelper.Settings;
        Log.A($"Start with PageState for Page:{pageFactory.Current.PageId}; Level:1");

        // update dependent properties
        MenuPages = PageFactory.UserPages.ToList();
        Debug = [];
    }

    //internal Log Log { get; }

    //internal MagicMenuPageSetHelper SetHelper { get; }
    //internal MagicPageFactory PageFactory { get; }
    //public MagicMenuSettings Settings { get; }

    #region Init

    public MagicMenuTree Setup(MagicMenuSettings? settings)
    {
        Log.A($"Init MagicMenuSettings Start:{settings?.Start}; Level:{settings?.Level}");
        if (settings != null) 
            ((MagicMenuPageSetHelper)SetHelper).Set(settings);
        return this;
    }

    public MagicMenuTree SetMenuPages(IEnumerable<MagicPage> menuPages)
    {
        Log.A($"Init menuPages:{menuPages.Count()}");
        MenuPages = menuPages.ToList();
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
        SetHelper.Set(pageDesigner);
        return this;
    }

    #endregion

    /// <summary>
    /// Pages in the menu according to Oqtane pre-processing
    /// Should be limited to pages which should be in the menu, visible and permissions ok. 
    /// </summary>
    internal IList<MagicPage> MenuPages { get; private set; }

    internal override MagicMenuTree Tree => this;

    public int MaxDepth => _maxDepth ??= Settings?.Depth ?? MagicMenuSettings.LevelDepthFallback;
    private int? _maxDepth;

    public List<string> Debug { get; private set; }

    protected override List<MagicPage> GetChildPages() => _rootPages ??= GetRootPages();
    private List<MagicPage>? _rootPages;

    protected List<MagicPage> GetRootPages()
    {
        var l = Log.Fn<List<MagicPage>>();
        // Give empty list if we shouldn't display it
        var result = new NodeRuleHelper(PageFactory, MenuPages, PageFactory.Current, Settings, Log).GetRootPages();
        return l.Return(result);
    }


    //private ITokenReplace TokenReplace => _nodeReplace ??= SetHelper.PageTokenEngine(this);
    //private ITokenReplace? _nodeReplace;

    ///// <inheritdoc cref="IMagicPageList.Classes" />
    //public string? Classes(string tag) => TokenReplace.Parse(SetHelper.Design.Classes(tag, this)).EmptyAsNull();

    ///// <inheritdoc cref="IMagicPageList.Value" />
    //public string? Value(string key) => TokenReplace.Parse(SetHelper.Design.Value(key, this)).EmptyAsNull();

}
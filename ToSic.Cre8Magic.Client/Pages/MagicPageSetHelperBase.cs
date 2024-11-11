﻿using ToSic.Cre8magic.Client.Pages.Internal;
using ToSic.Cre8magic.Pages;

namespace ToSic.Cre8magic.Client.Pages;

internal abstract class MagicPageSetHelperBase(MagicPageFactory pageFactory)
{
    #region Logging

    internal LogRoot LogRoot { get; } = pageFactory.LogRoot;

    internal Log Log => _log ??= LogRoot.GetLog(GetType().Name);
    private Log? _log;

    #endregion

    #region Abstract

    public abstract IMagicPageSetSettings Settings { get; }

    #endregion

    protected abstract IPageDesigner FallbackDesigner();
    public void Set(IPageDesigner designer) => _designer = designer;

    internal IPageDesigner Design => _designer ??= FallbackDesigner();
    private IPageDesigner? _designer;


    public void Set(MagicSettings magicSettings) => MagicSettings = magicSettings;

    internal MagicSettings? MagicSettings { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="page">The page which would be used if any page property is requested</param>
    /// <returns></returns>
    internal TokenEngine PageTokenEngine(IMagicPage page)
    {
        // fallback without MagicSettings return just TokenEngine with PageTokens
        if (MagicSettings == null)
            return new TokenEngine([new PageTokens(page)]);

        var originalPage = (PageTokens)MagicSettings.Tokens.Parsers.First(p => p.NameId == PageTokens.NameIdConstant);
        originalPage = originalPage.Clone(page);
        return MagicSettings.Tokens.SwapParser(originalPage);
    }


    #region Children

    /// <summary>
    /// Retrieve the children the first time it's needed.
    ///
    /// It's virtual, since other SetHelpers will have different implementations.
    /// For exapmle the MagicMenuPageSetHelper will stop if a certain depth has been reached.
    /// </summary>
    /// <returns></returns>
    public virtual List<IMagicPageWithDesignWip> GetChildren(IMagicPage page)
    {
        var l = Log.Fn<List<IMagicPageWithDesignWip>>($"{nameof(page.MenuLevel)}: {page.MenuLevel}");

        var children = pageFactory.ChildrenOf(page.Id)
            .Select(child => new MagicPageWithDesign(pageFactory, this, child, page.MenuLevel + 1) as IMagicPageWithDesignWip)
            .ToList();
        return l.Return(children, $"{children.Count}");
    }


    #endregion
}
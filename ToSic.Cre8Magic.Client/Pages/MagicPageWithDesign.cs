﻿using ToSic.Cre8magic.Client.Models;

namespace ToSic.Cre8magic.Client.Pages;

public class MagicPageWithDesign : MagicPage
{
    /// <param name="pageFactory"></param>
    /// <param name="setHelper"></param>
    /// <param name="page">The original page.</param>
    internal MagicPageWithDesign(MagicPageFactory pageFactory, MagicPageSetHelperBase setHelper, MagicPage? page = null) : base(page?.OriginalPage ?? pageFactory.PageState.Page, pageFactory)
    {
        SetHelper = setHelper;
    }

    internal MagicPageSetHelperBase SetHelper { get; }

    private ITokenReplace TokenReplace => _nodeReplace ??= SetHelper.PageTokenEngine(this);
    private ITokenReplace? _nodeReplace;

    /// <summary>
    /// Get css class for tag.
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public string? Classes(string tag) => TokenReplace.Parse(SetHelper.Design.Classes(tag, this)).EmptyAsNull();

    /// <summary>
    /// Get attribute value.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string? Value(string key) => TokenReplace.Parse(SetHelper.Design.Value(key, this)).EmptyAsNull();

}
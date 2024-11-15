﻿using Microsoft.AspNetCore.Components;
using ToSic.Cre8magic.Utils;

namespace ToSic.Cre8magic.Containers;

public partial class MagicContainer: Oqtane.Themes.ContainerBase
{
    /// <summary>
    /// Visible name in the UI (unless overridden again in the inheriting container)
    /// </summary>
    public override string Name => "Default (for Content and Admin)";

    public virtual MagicContainerSettings? Settings => null;

    [Inject]
    public IMagicFactoryWip MagicFactory { get; set; }

    #region Navigation / Close

    [Inject]
    public NavigationManager? NavigationManager { get; set; }

    public string CloseUrl { get; private set; } = "#";

    /// <summary>
    /// This should update the URL.
    /// But it only works on the "first" popup-dialog, anything after that will have a real url in the path and will
    /// actually just point to the same page.
    ///
    /// TODO: figure out a better way to remember the way back...
    /// </summary>
    protected override void OnParametersSet() => 
        CloseUrl = !string.IsNullOrEmpty(PageState.ReturnUrl) ? PageState.ReturnUrl : NavigateUrl();

    #endregion

    private MagicContainerDesigner Designer => _designer ??= MagicFactory.ContainerDesigner(PageState, ModuleState);
    private MagicContainerDesigner? _designer;

    public string? Classes(string target) => Designer.Classes(target);
    public string? Id(string name) => Designer.Id(name);
    public string? Value(string key) => Designer.Value(key);

    /// <summary>
    /// Modules are treated as admin modules (and must use the admin container) if they are marked as such, or come from the Oqtane ....Admin... type
    /// </summary>
    /// <returns></returns>
    protected bool UseAdminContainer => ModuleState.ForceAdminContainer();

}
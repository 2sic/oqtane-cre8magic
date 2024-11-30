﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using ToSic.Cre8magic.Act;
using ToSic.Cre8magic.Containers;

namespace ToSic.Cre8magic.OqtaneBs5;

/// <summary>
/// The recommended base container for all themes which use cre8magic.
///
/// It will simply remain invisible for the normal user,
/// and except for a DIV containing some classes, it will not render anything.
///
/// Admin-users will see a bit more, since the title etc. is necessary for the editing tools.
/// </summary>
/// <returns>
/// Despite cre8magic recommending for composition over inheritance,
/// this is a case we are forced to inherit the large object from Oqtane, because of the way Oqtane works.
/// We may some day choose to change this, if Oqtane improves it's architecture.
/// </returns>
public partial class MagicContainer: Oqtane.Themes.ContainerBase
{
    /// <summary>
    /// Visible name in the UI (unless overridden again in the inheriting container)
    /// </summary>
    public override string Name => "Default (for Content and Admin)";

    /// <summary>
    /// Settings for this container.
    /// Defaults to null, in which case it asks the theme etc. for settings.
    /// Inheriting code could overwrite it, to specify settings directly.
    /// </summary>
    public virtual MagicContainerSettings? Settings => null;

    [Inject] public required IMagicHat MagicHat { get; set; }

    #region Navigation / Close

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

    /// <summary>
    /// some comments
    /// </summary>
    [field: AllowNull, MaybeNull]
    protected IMagicContainerKit ContainerKit => field ??= MagicHat.ContainerKit(Settings.With(PageState, ModuleState));
}
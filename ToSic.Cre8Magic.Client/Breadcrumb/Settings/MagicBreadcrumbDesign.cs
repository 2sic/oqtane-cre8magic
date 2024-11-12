﻿using ToSic.Cre8magic.Settings;

namespace ToSic.Cre8magic.Breadcrumb.Settings;

public class MagicBreadcrumbDesign : DesignSetting
{
    ///// <summary>
    ///// List of classes to add on certain levels only.
    ///// Use level -1 to specify classes to apply to all the remaining ones which are not explicitly listed.
    ///// </summary>
    //public Dictionary<int, string>? ByLevel { get; set; }

    /// <summary>
    /// Classes to add if this node is a parent (has-children).
    /// </summary>
    public PairOnOff? HasChildren { get; set; }

    /// <summary>
    /// Classes to add if the node is disabled.
    /// TODO: unclear why it's disabled, what would cause this...
    /// </summary>
    public PairOnOff? IsDisabled { get; set; }

    ///// <summary>
    ///// Classes to add if this node is in the path / breadcrumb of the current page.
    ///// </summary>
    //public PairOnOff? InBreadcrumb { get; set; }
}
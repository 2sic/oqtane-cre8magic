﻿using Microsoft.AspNetCore.Components;
using Oqtane.Themes;

namespace ToSic.Cre8Magic.Client.Controls;

public abstract class MagicControl: ThemeControlBase, IMagicControlWithSettings
{
    [CascadingParameter] public MagicSettings Settings { get; set; }
}
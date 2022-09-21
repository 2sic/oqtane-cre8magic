﻿using Microsoft.AspNetCore.Components;

namespace ToSic.Cre8Magic.Client.Controls;

public interface IMagicControlWithSettings
{
    [CascadingParameter] MagicSettings Settings { get; set; }

    public string? Classes(string target);

    public string? Value(string target);
}
﻿namespace ToSic.Cre8magic.ComponentsBs5;

public class MagicDynamicComponent(string group, Type type, Dictionary<string, object>? parameters)
{
    public string Group { get; set; } = group;

    public Type Type { get; set; } = type;

    public Dictionary<string, object>? Parameters { get; set; } = parameters;
}
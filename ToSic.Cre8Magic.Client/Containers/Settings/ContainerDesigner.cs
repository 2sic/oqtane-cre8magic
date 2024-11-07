﻿using Oqtane.Models;

namespace ToSic.Cre8magic.Client.Containers.Settings;

internal class ContainerDesigner(MagicSettings settings, Module module) : ThemeDesigner(settings)
{
    protected override TokenEngine Tokens => _tokens1 ??= Settings.Tokens.Expanded(new ModuleTokens(module));
    private TokenEngine? _tokens1;

    public override string? Classes(string tag)
    {
        if (GetSettings(tag) is not { } styles) return null;
        var value = CombineWithModuleClasses(styles);
        return PostProcess(value);
    }


    /// <summary>
    /// Replace for container design rules
    /// </summary>
    /// <param name="styles"></param>
    /// <returns></returns>
    private string CombineWithModuleClasses(DesignSetting styles)
    {
        var value =  string.Join(" ", new[]
        {
            styles.Classes,
            styles.IsPublished.Get(module.IsPublished()),      // Info-Class if not published
            styles.IsAdmin.Get(module.ForceAdminContainer())   // Info-class if admin module
        }.Where(s => s.HasValue()));

        return value;
    }
}
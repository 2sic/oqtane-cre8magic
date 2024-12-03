﻿using System.Collections;
using ToSic.Cre8magic.Pages;
using ToSic.Cre8magic.Settings;
using ToSic.Cre8magic.Settings.Internal;
using ToSic.Cre8magic.Utils;
using ToSic.Cre8magic.Utils.Logging;

namespace ToSic.Cre8magic.Breadcrumbs;

/// <summary>
/// Special helper to provide Css classes to menus
/// </summary>
public class MagicBreadcrumbTailor : IMagicPageTailor
{
    internal MagicBreadcrumbTailor(WorkContext workContext, MagicBreadcrumbSettings settings)
    {
        BreadcrumbSettings = settings ?? throw new ArgumentException("BreadcrumbConfig must be real", nameof(BreadcrumbSettings));

        DesignSettingsList = [BreadcrumbSettings.Blueprint ?? new()];

        Log = workContext.LogRoot.GetLog("magic-breadcrumb-designer");
    }

    private Log Log { get; }

    private MagicBreadcrumbSettings BreadcrumbSettings { get; }
    internal List<MagicBreadcrumbBlueprint> DesignSettingsList { get; }


    public string Classes(string tag, IMagicPage item)
    {
        var configsForTag = ConfigsForTag(tag);
        return configsForTag.Any()
            ? ListToClasses(TagClasses(item, configsForTag))
            : "";
    }

    public string Value(string key, IMagicPage item)
    {
        var configsForKey = ConfigsForTag(key)
            .Select(c => c.Value)
            .Where(v => v.HasValue())
            .ToList();

        return string.Join(" ", configsForKey);
    }

    private List<MagicBlueprintPart> ConfigsForTag(string tag) =>
        DesignSettingsList
            .Select(c => c?.Parts?.FindInvariant(tag))
            .Where(c => c is not null)
            .ToList()!;

    private List<string?> TagClasses(IMagicPage page, IReadOnlyCollection<MagicBlueprintPart> configs)
    {
        var classes = new List<string?>();

        void AddIfAny(IEnumerable<string?> maybeAdd)
        {
            var additions = maybeAdd.Where(v => v != null).ToList();
            if (additions.Any()) classes.AddRange(additions);
        }

        AddIfAny(configs.Select(c => c.Classes));
        AddIfAny(configs.Select(c => c.Classes));
        AddIfAny(configs.Select(c => c.IsActive.Get(page.IsActive)));
        AddIfAny(configs.Select(c => c.HasChildren.Get(page.HasChildren)));
        AddIfAny(configs.Select(c => c.IsDisabled.Get(!page.IsClickable)));
        //AddIfAny(configs.Select(c => c.InBreadcrumb.Get(page.InBreadcrumb)));

        //// See if there are any css for this level or for not-specified levels
        //var levelCss = configs
        //    .Select(c => c.ByLevel == null
        //        ? null
        //        : c.ByLevel.TryGetValue(page.Level, out var levelClasses)
        //            ? levelClasses
        //            : c.ByLevel.TryGetValue(MagicTokens.ByLevelOtherKey, out var levelClassesDefault)
        //                ? levelClassesDefault
        //                : null);
        //AddIfAny(levelCss);

        return classes;
    }

    private string ListToClasses(IEnumerable<string?> original)
        => string.Join(" ", original.Where(s => !s.IsNullOrEmpty())).Replace("  ", " ");
}
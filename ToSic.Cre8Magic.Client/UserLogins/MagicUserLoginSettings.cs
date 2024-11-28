﻿using System.Text.Json.Serialization;
using ToSic.Cre8magic.Settings;
using ToSic.Cre8magic.Settings.Internal;

namespace ToSic.Cre8magic.UserLogins;

public record MagicUserLoginSettings : MagicSettingsBase, ICanClone<MagicUserLoginSettings>
{
    /// <summary>
    /// Dummy constructor so better find cases where it's created
    /// Note it must be without parameters for json deserialization
    /// </summary>
    [PrivateApi]
    public MagicUserLoginSettings() {}

    private MagicUserLoginSettings(MagicUserLoginSettings? priority, MagicUserLoginSettings? fallback = default)
        : base(priority, fallback)
    {
        DesignSettings = priority?.DesignSettings ?? fallback?.DesignSettings;
    }

    MagicUserLoginSettings ICanClone<MagicUserLoginSettings>.CloneUnder(MagicUserLoginSettings? priority, bool forceCopy = false) =>
        priority == null ? (forceCopy ? this with { } : this) : new(priority, this);


    [JsonIgnore]
    public MagicLanguageDesignSettings? DesignSettings { get; init; }


    //internal static Defaults<MagicUserLoginSettings> Defaults = new()
    //{
    //    Fallback = new()
    //    {
    //        HideOthers = false,
    //        MinLanguagesToShow = 2,
    //        Languages = new()
    //        {
    //            { "en", new() { Culture = "en", Description = "English" } }
    //        },
    //    },
    //    Foundation = new()
    //    {
    //        HideOthers = false,
    //        Languages = new(),
    //    }
    //};
}
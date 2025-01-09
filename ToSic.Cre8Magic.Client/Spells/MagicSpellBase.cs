﻿using System.Text.Json.Serialization;
using Oqtane.UI;
using ToSic.Cre8magic.Spells.Debug;
using ToSic.Cre8magic.Spells.Internal.Debug;
using ToSic.Cre8magic.Spells.Internal.Docs;

namespace ToSic.Cre8magic.Spells;

/// <summary>
/// Internal base class containing all kinds of settings which
/// all spells share.
/// </summary>
public abstract record MagicSpellBase: MagicInheritsBase, ISettingsForCodeUse, IHasDebugSettings, IDebugSettings
{
    #region Constructor & Cloning

    [PrivateApi]
    protected MagicSpellBase() { }

    [PrivateApi]
    protected MagicSpellBase(MagicSpellBase? priority, MagicSpellBase? fallback = default)
        : base(priority, fallback)
    {
        if (fallback == null)
            return;

        Name = priority?.Name ?? fallback.Name;
        BlueprintName = priority?.BlueprintName ?? fallback.BlueprintName;

        Debug = priority?.Debug ?? fallback.Debug;

        // Page State
        PageState = priority?.PageState ?? fallback.PageState;

        // Debug settings
        ((IDebugSettings)this).Book = ((IDebugSettings?)priority)?.Book ?? ((IDebugSettings?)fallback)?.Book;
        ((IDebugSettings)this).DebugThis = ((IDebugSettings?)priority)?.DebugThis ?? ((IDebugSettings?)fallback)?.DebugThis;
    }

    #endregion


    #region Settings for Code

    /// <summary>
    /// The PageState which is needed for doing everything.
    ///
    /// It can be provided in the settings, or it must be provided in the theme using <see cref="ToSic.Cre8magic.Act.IMagicAct.UsePageState"/>.
    /// </summary>
    [JsonIgnore]
    public PageState? PageState { get; init; }


    /// <inheritdoc/>
    [JsonIgnore]
    public string? Name { get; init; }

    /// <inheritdoc/>
    [JsonIgnore]
    public string? BlueprintName { get; init; }

    #endregion

    #region Debug Settings (from store)

    /// <inheritdoc />
    public MagicDebugSettings? Debug { get; init; }


    #endregion

    #region Runtime Debug Settings

    [JsonIgnore]
    MagicSpellsBook? IDebugSettings.Book { get; set; }

    [JsonIgnore]
    bool? IDebugSettings.DebugThis { get; set; }

    #endregion
}
﻿using Microsoft.AspNetCore.Components;

namespace ToSic.Cre8Magic.Client.Containers;

public class MagicContainer: Oqtane.Themes.ContainerBase, IMagicControlWithSettings
{
    [CascadingParameter] public MagicSettings Settings { get; set; }

    [Inject] public NavigationManager? NavigationManager { get; set; }

    public int ModuleId => ModuleState.ModuleId;

    protected void CloseModal() => NavigationManager?.NavigateTo(NavigateUrl());

    public string? Classes(string tag) => Settings.ContainerDesign.Classes(Settings, ModuleState, tag).EmptyAsNull();

    public string? Value(string key) => Settings.Container.Value(Settings, ModuleState, key).EmptyAsNull();

    /// <summary>
    /// Modules are treated as admin modules (and must use the the admin container) if they are marked as such, or come from the Oqtane ....Admin... type
    /// </summary>
    /// <returns></returns>
    protected bool ForceAdminContainer => ModuleState.ForceAdminContainer();

}
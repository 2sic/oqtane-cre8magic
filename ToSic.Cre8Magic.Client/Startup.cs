﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ToSic.Cre8magic.Analytics;
using ToSic.Cre8magic.Pages;
using ToSic.Cre8magic.Pages.Internal;
using ToSic.Cre8magic.Services.Internal;
using ToSic.Cre8magic.Settings.Internal;
using ToSic.Cre8magic.Settings.Json;

namespace ToSic.Cre8magic;

public class Startup : Oqtane.Services.IClientStartup
{
    /// <summary>
    /// Register Services
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        // All these Settings etc. should be scoped, so they don't have to reload for each click
        //services.TryAddScoped<MagicSettingsService>();
        services.TryAddScoped<IMagicSettingsService, MagicSettingsService>();

        // Services used by SettingsService, which is scoped, so the dependencies can be normal transient
        services.TryAddTransient<MagicSettingsLoader>();
        services.TryAddTransient<MagicSettingsJsonService>();

        services.TryAddTransient<LanguageService>();

        services.TryAddTransient<MagicThemeJsServiceTest>();

        // Logic parts for Controls
        services.TryAddTransient<MagicPageEditService>();

        // Analytics - new in 0.0.2
        services.TryAddTransient<MagicAnalyticsService>();

        services.TryAddTransient<MagicMenuBuilder>();

        //services.TryAddTransient<MagicPageService>(); // Can't DI because of PageState dependency that breaks Oqtane 
        //services.TryAddTransient<MagicMenuTree>(); // Can't DI because of PageState dependency that breaks Oqtane 

        // WIP v0.02.00
        services.TryAddTransient<IMagicPageService, MagicPageService>();
    }
}
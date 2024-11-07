﻿using System.Collections.Generic;
using ToSic.Cre8magic.Client.Models;
using ToSic.Cre8magic.Client.Pages;

namespace ToSic.Theme.Cre8magicTests.Client;

internal class CustomBreadcrumbDesigner : IPageDesigner
{
    public string Classes(string tag, MagicPage item)
    {
        // List to store CSS class names
        var classes = new List<string> { "custom" };

        // Additional classes based on the HTML tag
        switch (tag.ToLower())
        {
            case "ol":
                // Use 'breadcrumb' class from Bootstrap
                classes.Add("breadcrumb");
                classes.Add("bg-dark");
                classes.Add("text-white");
                break;

            case "li":
                // Use 'breadcrumb-item' class from Bootstrap
                classes.Add("breadcrumb-item");
                if (item.IsCurrent) classes.Add("active");
                break;

            case "a" or "span":
                classes.Add("text-white"); ;
                break;

            default:
                // Handle any other tags if necessary
                break;
        }

        // Return the CSS classes as a space-separated string
        return string.Join(" ", classes);
    }

    public string Value(string key, MagicPage item) 
        => key.ToLower() switch
        {
            "aria-current" => item.IsCurrent ? "page" : "",
            _ => ""
        };
}
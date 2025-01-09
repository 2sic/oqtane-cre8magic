---
uid: Cre8magic.Library.MagicSettings.SettingsLookupWithName
---

# cre8magic - Magic Settings Lookup With Name WIP

WIP trying to document how the settings are looked up.

## Named Parts

Every part _can_ have a name.
This name is used to look up the settings for the part.

## Goals

1. Every part is named / identified
1. ...and if not, a default name/identifier is used...
1. The name is used to look up the spell
1. The name is used to look up the blueprints
1. Theme Variants can "redirect" the name(s)
1. The settings can be reused and expanded by another name (`@inherits`)


## Simple Use Case

Let's look at a menu which has the name `MainMenu`.

```html
`<MagicMenu Name="MainMenu" />`
```

The system will then use the **MagicSpellsBook** in the **MagicThemePackage** and look for:

* Menus (Spells) with the name `MainMenu`
* MenuBlueprints with the name `MainMenu`

These will then be used to create the menu.

## Advanced Use Case with Multiple Theme Variants

Let's say you have a theme with multiple variants, such as `FixedWidth` and `FullScreen`.
In this case it could be that the `MainMenu` is different for each variant - and some things such as the `SideMenu` may not even appear.

In most of these cases, the Razor will actually stay the same:

```html
`<MagicMenu Name="MainMenu" />`
...
`<MagicMenu Name="SideMenu" />`
```

...but the Settings-Lookup will have an additional step:

1. Check the **ThemeSpell** in the **SpellBook** and look at the Parts-Map
1. If the `MainMenu` is found in the Map, then check if there are renames. Let's assume that
    1. The `FixedWidth` theme maps the design to `MainMenuFixedWidth` and leaves the Spell-Settings as is
    1. The `FullScreen` leaves everything as is
1. The new names are then used to find the Spells and Blueprints
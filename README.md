# Scriptable Settings Provider

* This package makes easier to create Settings Menus.
* Unity minimum version: **2019.3**
* Current version: **1.1.0**
* License: **MIT**

## Summary

Using this package you can create Settings Menus by extending the [AbstractScriptableSettingsProvider](/Editor/AbstractScriptableSettingsProvider.cs) class.

## How To Use

First, create a class extending from ScriptableObject:

```csharp
using UnityEngine;

public sealed class TestScriptableMenu : ScriptableObject
{
    [Min(10)] public int id;
    [Range(0F, 1F)] public float range;
    [TextArea] public string description;
}
```

Now, **on an Editor folder**, create a class implementing **AbstractScriptableSettingsProvider**, with a static `CreateProjectSettingsMenu` function:

```csharp
using UnityEditor;
using ActionCode.ScriptableSettingsProvider.Editor;

public sealed class TestScriptableMenuProvider :
    AbstractScriptableSettingsProvider<TestScriptableMenu>
{
    public TestScriptableMenuProvider() :
        base("Your Menu Category/Scriptable Test")
    {
    }

    protected override string GetConfigName() => "com.yourpackage.name";

    [SettingsProvider]
    private static SettingsProvider CreateProjectSettingsMenu() =>
            new TestScriptableMenuProvider();
}
```

Open the **Project Settings** Windows and check your new menu entry:

![The Scriptable Test Menu](/Docs~/ScriptableTestMenu.png "The Scriptable Test Menu")

## Preloaded Asset

Preloaded Assets are assets that will be loaded when the game starts and they will be kept in memory until the game finishes.

This is useful to store data that can be accessed at any time, like a global Game Settings and you can easily load this data by using `Resources.Load()` function.

To be able to do this, you must first add your Object reference into the **Preloaded Asset** list, located at **Project Settings**, **Optimization** section.

![The Preloaded Assets List](/Docs~/PreloadedAssetsList.png "The Preloaded Assets List")

### Creating Settings as Preloaded Asset

You can mark a Settings Provider class as a Preload Asset and, every time the **Current Settings** property is edited inside your menu, the settings reference inside the Preloaded Asset list will be edited as well.

To do this, just pass the `isPreloadedAsset` parameter as `true` inside your menu provider class, like so:

```csharp
    public TestScriptableMenuProvider() :
        base("Your Menu Category/Scriptable Test", isPreloadedAsset: true)
    {
    }
```

Now `TestScriptableMenu` will be automatically instantiate when your game starts. To get a singleton access to it, you could do something like this: 

```csharp
public sealed class TestScriptableMenu : ScriptableObject
{
    public static TestScriptableMenu Instance { get; private set; }

    private void OnEnable() => Instance = this;
}
```

## Installation

### Using the Package Registry Server

Follow the instructions inside [here](https://cutt.ly/ukvj1c8) and the package **ActionCode-Scriptable Settings Provider** 
will be available for you to install using the **Package Manager** windows.

### Using the Git URL

You will need a **Git client** installed on your computer with the Path variable already set. 

- Use the **Package Manager** "Add package from git URL..." feature and paste this URL: `https://github.com/HyagoOliveira/ScriptableSettingsProvider.git`

- You can also manually modify you `Packages/manifest.json` file and add this line inside `dependencies` attribute: 

```json
"com.actioncode.scriptable-settings-provider":"https://github.com/HyagoOliveira/ScriptableSettingsProvider.git"
```

---

**Hyago Oliveira**

[GitHub](https://github.com/HyagoOliveira) -
[BitBucket](https://bitbucket.org/HyagoGow/) -
[LinkedIn](https://www.linkedin.com/in/hyago-oliveira/) -
<hyagogow@gmail.com>
# Scriptable Settings Provider

* This package makes easier to create Settings Menus.
* Unity minimum version: **2019.3**
* Current version: **0.1.0**
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

Now, **on an Editor folder**, create class implementing AbstractScriptableSettingsProvider, with a static `CreateProjectSettingsMenu` function:

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

![The Scriptable Test Menu](/Docs~/ScriptableTestMenu.png "The Scriptable Test Menu").

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
using System;
using System.Collections.Generic;
using HotRepl.Control;
using MelonLoader;
using MyMod;

[assembly: MelonInfo(
    typeof(MyMod.MelonLoader.MyModMelonMod),
    "MyMod",
    "0.1.0",
    "AUTHOR_PLACEHOLDER"
)]

namespace MyMod.MelonLoader;

/// <summary>MelonLoader entry point for this HotRepl command plugin.</summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1001:Types that own disposable fields should be disposable",
    Justification = "MelonLoader owns the mod lifecycle; OnDeinitializeMelon disposes command registrations."
)]
public sealed class MyModMelonMod : MelonMod
{
    private readonly List<IDisposable> _registrations = new();
    private MelonPreferences_Entry<bool> _enabled;

    /// <inheritdoc />
    public override void OnInitializeMelon()
    {
        var category = MelonPreferences.CreateCategory("MyMod");
        _enabled = category.CreateEntry(
            "Enabled",
            true,
            description: "Master switch. Changes apply on next game start."
        );
    }

    /// <inheritdoc />
    public override void OnLateInitializeMelon()
    {
        if (!_enabled.Value)
        {
            LoggerInstance.Msg("MyMod disabled via config.");
            return;
        }

        foreach (var factory in MyModCatalog.Build())
        {
            _registrations.Add(factory(GlobalControlCommandRegistry.Instance));
        }

        LoggerInstance.Msg($"MyMod registered {_registrations.Count} commands.");
    }

    /// <inheritdoc />
    public override void OnDeinitializeMelon()
    {
        foreach (var registration in _registrations)
        {
            registration.Dispose();
        }

        _registrations.Clear();
    }
}

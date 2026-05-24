using System;
using System.Collections.Generic;
using BepInEx;
using HotRepl.Control;
using MyMod;

namespace MyMod.BepInEx;

/// <summary>BepInEx entry point for this HotRepl command plugin.</summary>
[BepInPlugin(PluginInfo.PluginGuid, PluginInfo.PluginName, PluginInfo.PluginVersion)]
[BepInDependency("hotrepl.bepinex", BepInDependency.DependencyFlags.HardDependency)]
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1001:Types that own disposable fields should be disposable",
    Justification = "BepInEx owns the plugin lifecycle; OnDestroy disposes command registrations."
)]
public sealed class Plugin : BaseUnityPlugin
{
    private readonly List<IDisposable> _registrations = new();

    private void Awake()
    {
        var enabled = Config.Bind(
            "General",
            "Enabled",
            true,
            "Master switch. Changes apply on next game start."
        );

        if (!enabled.Value)
        {
            Logger.LogInfo($"{PluginInfo.PluginName} disabled via config.");
            return;
        }

        foreach (var factory in MyModCatalog.Build())
        {
            _registrations.Add(factory(GlobalControlCommandRegistry.Instance));
        }

        Logger.LogInfo($"{PluginInfo.PluginName} registered {_registrations.Count} commands.");
    }

    private void OnDestroy()
    {
        foreach (var registration in _registrations)
        {
            registration.Dispose();
        }

        _registrations.Clear();
    }
}

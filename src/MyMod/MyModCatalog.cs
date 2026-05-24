using System;
using System.Collections.Generic;
using HotRepl.Control;
using MyMod.Commands;
using MyMod.Models;

namespace MyMod;

/// <summary>Registers every command exposed by this mod.</summary>
public static class MyModCatalog
{
    /// <summary>Builds registration factories for every command.</summary>
    public static IReadOnlyList<RegistrationFactory> Build() =>
        new RegistrationFactory[]
        {
            registry =>
                registry.Register<HelloWorldArgs, HelloWorldResult>(new HelloWorldCommand()),
        };

    /// <summary>Registers one command and returns the disposable registration.</summary>
    public delegate IDisposable RegistrationFactory(IControlCommandRegistry registry);
}

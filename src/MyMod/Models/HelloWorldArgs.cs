using System.ComponentModel;

namespace MyMod.Models;

/// <summary>Arguments for the demo hello-world command.</summary>
public sealed class HelloWorldArgs
{
    /// <summary>Optional name. Empty or whitespace defaults to <c>world</c>.</summary>
    [Description("Optional name. Empty or whitespace defaults to 'world'.")]
    public string Name { get; set; } = "";
}

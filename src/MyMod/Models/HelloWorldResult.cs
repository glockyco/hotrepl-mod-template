using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace MyMod.Models;

/// <summary>Result returned by the demo hello-world command.</summary>
public sealed class HelloWorldResult
{
    /// <summary>The greeting that was produced.</summary>
    [Description("The greeting that was produced.")]
    [JsonProperty("greeting", Required = Required.Always)]
    public string Greeting { get; set; } = "";

    /// <summary>Server-side timestamp when the greeting was generated.</summary>
    [Description("Server-side timestamp when the greeting was generated, ISO 8601 UTC.")]
    [JsonProperty("generatedAt", Required = Required.Always)]
    public DateTimeOffset GeneratedAt { get; set; }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using HotRepl.Control;
using MyMod.Models;

namespace MyMod.Commands;

/// <summary>
/// Demo command — replace with your own.
///
/// The canonical typed-command pattern:
/// 1. Define POCO args and result types (see Models/).
/// 2. Implement IControlCommandHandler&lt;TArgs, TResult&gt;.
/// 3. Register in MyModCatalog so both loader plugins expose it.
///
/// Invoke from any HotRepl client:
/// $ hotrepl run mymod.hello '{}'
/// $ hotrepl run mymod.hello '{"name":"World"}'
/// </summary>
public sealed class HelloWorldCommand : IControlCommandHandler<HelloWorldArgs, HelloWorldResult>
{
    /// <inheritdoc />
    public string Name => "mymod.hello";

    /// <inheritdoc />
    public int Version => 1;

    /// <inheritdoc />
    public ControlCommandKind Kind => ControlCommandKind.Synchronous;

    /// <inheritdoc />
    public bool MutatesState => false;

    /// <inheritdoc />
    public ValueTask<ControlCommandResult<HelloWorldResult>> ExecuteAsync(
        ControlCommandContext context,
        HelloWorldArgs args,
        CancellationToken cancellationToken
    )
    {
        var who = string.IsNullOrWhiteSpace(args.Name) ? "world" : args.Name;
        return new(
            ControlCommandResult.Ok(
                new HelloWorldResult
                {
                    Greeting = $"Hello, {who}!",
                    GeneratedAt = DateTimeOffset.UtcNow,
                }
            )
        );
    }
}

# hotrepl-mod-template

Scaffold for writing a [HotRepl](https://github.com/glockyco/HotRepl) command plugin for a Unity
game. The template supports BepInEx 5 on Mono and MelonLoader on IL2CPP by compiling the same shared
command source into loader-specific entrypoint assemblies.

## What you get

- One typed demo command, `mymod.hello`, implemented with `IControlCommandHandler<TArgs, TOutput>`.
- BepInEx and MelonLoader loader projects that register the same command catalog.
- Deploy scripts and local path configuration for copying the built plugin into a game install.
- Vendored HotRepl 3.0 reference assemblies under `vendor/hotrepl/` so the template builds before a
  public NuGet package exists.

## Prerequisites

- .NET SDK 10.x.
- A Unity game with either BepInEx 5.x or MelonLoader 0.6+ installed.
- The matching HotRepl host plugin loaded in the game:
  - `HotRepl.BepInEx.dll` for BepInEx.
  - `HotRepl.Host.MelonLoader.dll` for MelonLoader.
- The HotRepl CLI for testing (`bunx @hotrepl/cli` from the HotRepl repo).

## Quickstart

### Option 1: use the GitHub template

1. Click **Use this template** → **Create a new repository**.
2. Clone your new repository locally.
3. Rename `MyMod` and `mymod.hello` to your mod and command names.

### Option 2: use `dotnet new`

```bash
dotnet new install glockyco/hotrepl-mod-template
dotnet new hotrepl-mod -n AwesomeMod -o ~/Projects/AwesomeMod \
  --PluginGuid awesomestudios.awesomemod --Author "Awesome Studios"
```

`dotnet new` rewrites `mymod.hello` to the lowercase project-name prefix (for example,
`awesomemod.hello`). GitHub template users should rename `MyMod` and `mymod.hello` manually.

### Configure local paths and deploy

```bash
cp Local.props.example Local.props
# Edit Local.props and set either BepInExPluginPath + UnityManagedPath,
# or MelonLoaderPath + Il2CppAssembliesPath + MelonLoaderModsPath.
bash scripts/deploy-bepinex.sh       # or bash scripts/deploy-melonloader.sh
```

Launch the game, then from any HotRepl client:

```bash
bunx @hotrepl/cli describe mymod.hello
bunx @hotrepl/cli run mymod.hello '{"name":"World"}'
```

Expected result:

```json
{
  "greeting": "Hello, World!",
  "generatedAt": "..."
}
```

## Project layout

| Path | Purpose |
| --- | --- |
| `src/MyMod/` | Shared command handlers and POCO arg/result types. Compiled into both loader DLLs. |
| `src/MyMod.BepInEx/` | BepInEx plugin entrypoint. References Mono UnityEngine assemblies from `UnityManagedPath`. |
| `src/MyMod.MelonLoader/` | MelonLoader mod entrypoint. References IL2CPP-unhollowed assemblies from `Il2CppAssembliesPath`. |
| `vendor/hotrepl/` | HotRepl 3.0 reference assemblies used at compile time. The game should get runtime copies from the HotRepl host install. |
| `scripts/` | Build and deploy helpers. |
| `Local.props.example` | Copy to `Local.props` for machine-specific game paths. |

## Adding a command

1. Copy `src/MyMod/Commands/HelloWorldCommand.cs`.
2. Add or copy POCO arg/result types under `src/MyMod/Models/`.
3. Change the command `Name`, `Version`, `Kind`, and `MutatesState`.
4. Register the command in `src/MyMod/MyModCatalog.cs`.
5. Build and deploy; `bunx @hotrepl/cli describe <command>` will show the new descriptor and schema.

## Schema authoring

HotRepl uses standard .NET metadata on POCO properties:

| Attribute | Effect |
| --- | --- |
| `[Description("...")]` | Human-readable field description in `command_describe`. |
| `[Required]` | Required argument. Add `using System.ComponentModel.DataAnnotations;`. |
| `[Range(min, max)]` | Numeric bounds; invalid calls fail before the handler runs. |
| `[StringLength(max)]` | String length bounds. |
| `[JsonProperty("wireName")]` | Newtonsoft.Json wire-name override. |
| `[JsonIgnore]` | Hide a property from the schema and JSON payload. |

## Mono vs IL2CPP

Keep code in `src/MyMod/` when the same source compiles against both runtimes. If a command needs
loader-specific APIs, move that command into `src/MyMod.BepInEx/` or `src/MyMod.MelonLoader/` and
adapt `MyModCatalog.Build()` with `#if BEPINEX` / `#if MELONLOADER` registrations.

## Updating HotRepl references

The template currently vendors HotRepl 3.0 reference assemblies in `vendor/hotrepl/`. Replace those
DLLs with the same files from a newer HotRepl release when upgrading. Keep `Private=false` on the
references so your mod DLL does not carry a second copy of Core into the game; the HotRepl host owns
runtime deployment.

## License

MIT.

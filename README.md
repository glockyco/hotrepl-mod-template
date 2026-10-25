# hotrepl-mod-template

This template starts a Unity game mod that exposes typed commands through [HotRepl](https://github.com/glockyco/HotRepl). Use it when you want a CLI or coding agent to call your game-specific C# code through BepInEx on Mono or MelonLoader on IL2CPP.

The shared command source builds into either loader's plugin. The template includes a greeting command, loader entry points and deployment scripts. It does not install HotRepl itself.

## Start a mod

You need a .NET SDK compatible with the [CI workflow](.github/workflows/ci.yml), Bash for the deployment scripts, and a game with its mod loader and matching HotRepl host installed. Use Node.js and npm for the CLI examples below.

Choose "Use this template" on GitHub and create a repository, then clone it. You can keep the `MyMod` names until you have deployed the example command.

If you use the GitHub template, replace `PLUGIN_GUID_PLACEHOLDER` in [PluginInfo.cs](src/MyMod.BepInEx/PluginInfo.cs) and `AUTHOR_PLACEHOLDER` in [MyModMelonMod.cs](src/MyMod.MelonLoader/MyModMelonMod.cs) before deployment.

Alternatively, install the template from a local clone and generate a project:

```sh
git clone https://github.com/glockyco/hotrepl-mod-template.git
cd hotrepl-mod-template
dotnet new install .
dotnet new hotrepl-mod -n AwesomeMod -o ../AwesomeMod \
  --PluginGuid awesomestudios.awesomemod --Author "Awesome Studios"
cd ../AwesomeMod
```

The examples below use the original `MyMod` paths. In a generated project, use the generated names and check the command's `Name` in its handler before calling it.

## Configure and deploy

From your mod's repository root:

```sh
cp Local.props.example Local.props
```

Edit the paths in `Local.props` using [Local.props.example](Local.props.example) as a guide. Set the BepInEx plugin and managed assembly paths for Mono, or the MelonLoader, IL2CPP assembly and Mods paths for IL2CPP. `Local.props` is ignored by Git.

Close the game, then build and deploy for its loader:

```sh
# BepInEx on Mono
bash scripts/deploy-bepinex.sh

# Or MelonLoader on IL2CPP
bash scripts/deploy-melonloader.sh
```

The BepInEx script copies the mod DLL into a `MyMod` subfolder of your configured plugin directory. The MelonLoader script copies the mod DLL into your configured Mods directory. Both scripts overwrite an existing copy. They do not deploy the HotRepl host or its dependencies.

Launch the game, then inspect and run the example command:

```sh
npx -y @hotrepl/cli describe mymod.hello
npx -y @hotrepl/cli run mymod.hello '{"name":"World"}'
```

The command returns a greeting of `Hello, World!` and a generation timestamp. If it is missing, check the loader log and confirm that both the HotRepl host and your mod loaded.

## Add your commands

1. Rename `MyMod` throughout the project to your mod's name and set the plugin identity in the loader entry points. If you generated the project with `dotnet new`, inspect the generated identities before deployment.
2. Use [HelloWorldCommand.cs](src/MyMod/Commands/HelloWorldCommand.cs) as the handler example and add argument and result types under [Models](src/MyMod/Models).
3. Give the command a unique name and declare its metadata with `[ControlCommand]`. Keep its name, version, execution kind and state-mutation properties consistent with the attribute.
4. Register the handler in [MyModCatalog.cs](src/MyMod/MyModCatalog.cs).
5. Deploy again, restart the game and inspect the command with the CLI before calling it.

Keep shared game logic in [src/MyMod](src/MyMod). Put loader-specific code in [src/MyMod.BepInEx](src/MyMod.BepInEx) or [src/MyMod.MelonLoader](src/MyMod.MelonLoader).

The [HotRepl command authoring guide](https://github.com/glockyco/HotRepl/blob/main/docs/authoring-commands.md) covers schemas and runtime behavior. Handlers use `ControlCommandContext<TOutput>` and return results through helpers such as `context.Ok(output)`.

## References and checks

The loader project files reference [vendor/hotrepl](vendor/hotrepl) at compile time without copying those assemblies into the deployed mod. Use a compatible HotRepl host in the game. The [vendor README](vendor/hotrepl/README.md) records the source revision and build commands. When updating the references, replace the vendored set together and keep `Private=false` so the mod does not deploy a second Core assembly.

The [CI workflow](.github/workflows/ci.yml) checks template generation and compilation of the shared source. It does not build the loader projects against a game installation. Use the deployment script for your loader to build against your local game assemblies, then call your command in the running game.

## License

[MIT](LICENSE).

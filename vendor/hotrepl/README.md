# HotRepl compile-time references

The assemblies come from [HotRepl commit `839e8d47f58179d2967d79afaeb3b23e44d3c6ca`](https://github.com/glockyco/HotRepl/commit/839e8d47f58179d2967d79afaeb3b23e44d3c6ca).
The loader projects keep these references compile-time only with `Private=false` because the game must use the host's assemblies.

`NJsonSchema` and `Namotion.Reflection` are internalized into Core by HotRepl's build. Do not add separate references to those assemblies.

To reproduce the build, clone HotRepl, check out that commit, and follow its `AGENTS.md` setup inside `nix develop`:

```sh
git checkout 839e8d47f58179d2967d79afaeb3b23e44d3c6ca
nix develop
dotnet tool restore
bun install --frozen-lockfile
lefthook install
dotnet build src/HotRepl.Core/ -c Release --nologo -v q
dotnet build src/HotRepl.Protocol/ -c Release --nologo -v q
```

Replace the vendored DLLs together from `src/HotRepl.Core/bin/Release/netstandard2.1/`, including the matching dependency assemblies. Use the `netstandard2.1` output for both loader projects so the same references work with Mono and IL2CPP.

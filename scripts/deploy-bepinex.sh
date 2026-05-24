#!/usr/bin/env bash
# Builds the BepInEx loader and copies the DLL into the game's plugin folder.
set -euo pipefail

PLUGIN_PATH=$(dotnet msbuild src/MyMod.BepInEx/MyMod.BepInEx.csproj -nologo -getProperty:BepInExPluginPath)
if [[ -z "$PLUGIN_PATH" ]]; then
  echo "Set BepInExPluginPath in Local.props (copy from Local.props.example)." >&2
  exit 1
fi

dotnet build src/MyMod.BepInEx -c Debug --nologo -v q

DEST="$PLUGIN_PATH/MyMod"
mkdir -p "$DEST"
cp -f src/MyMod.BepInEx/bin/Debug/netstandard2.1/MyMod.BepInEx.dll "$DEST/"
echo "Deployed MyMod.BepInEx.dll to $DEST"

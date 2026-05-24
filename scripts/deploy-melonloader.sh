#!/usr/bin/env bash
# Builds the MelonLoader loader and copies the DLL into the game's Mods folder.
set -euo pipefail

MODS_PATH=$(dotnet msbuild src/MyMod.MelonLoader/MyMod.MelonLoader.csproj -nologo -getProperty:MelonLoaderModsPath)
if [[ -z "$MODS_PATH" ]]; then
  echo "Set MelonLoaderModsPath in Local.props (copy from Local.props.example)." >&2
  exit 1
fi

dotnet build src/MyMod.MelonLoader -c Debug --nologo -v q

mkdir -p "$MODS_PATH"

cp -f src/MyMod.MelonLoader/bin/Debug/net6.0/MyMod.MelonLoader.dll "$MODS_PATH/"
echo "Deployed MyMod.MelonLoader.dll to $MODS_PATH"

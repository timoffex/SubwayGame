#!/bin/bash

# Accepts one argument which is the path to this directory.
# This is used inside a Unity AssetPostprocessor script to add
# my FSharp projects to the .sln file.
#
# This has to use the absolute path to the dotnet command
# because I don't set the PATH.
/usr/local/share/dotnet/dotnet sln "$1"/SubwayPuzzle.sln add \
       "$1"/FSharp/FSharp.fsproj \
       "$1"/FSharp.Tests/FSharp.Tests.fsproj \
       >"$0.last_run" 2>&1

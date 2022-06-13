#!/usr/bin/env bash

function main() {
    dotnet publish .\JdSharp.Cli\ -c Release -v quiet -o jdsharp
}

main
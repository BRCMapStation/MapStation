Set-Location $PSScriptRoot/..
$ErrorActionPreference = 'Stop'

# `dotnet clean` doesn't delete everything.

Write-Output `
    .\libs\BRC-CommonAPI\bin `
    .\libs\BRC-CommonAPI\obj `
    .\Winterland.MapStation.Common\bin `
    .\Winterland.MapStation.Common\obj `
    .\Winterland.MapStation.Plugin\bin `
    .\Winterland.MapStation.Plugin\obj `
    .\Winterland.Common\obj `
    .\Winterland.Common\bin `
    .\Winterland.Common_Editor\obj `
    .\Winterland.Common_Editor\bin `
    .\Winterland.Plugin\obj `
    .\Winterland.Plugin\bin `
| ForEach-Object {
    if(test-path $_) { remove-item -recurse $_ }
}

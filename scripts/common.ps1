function GetJson($path) {
    $json = Get-Content $path | ConvertFrom-Json
    return $json
}
function PatchJson($path, $fn) {
    $json = GetJson $path
    # Trick to call $fn and set the automatic $_ var
    ForEach-Object -Process $fn -InputObject $json
    $json | ConvertTo-Json -Depth 99 | Set-Content $path
}

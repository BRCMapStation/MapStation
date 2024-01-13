# Assumes Unity manifest.json is configured to talk to http://localhost:8000/registry
Push-Location $PSScriptRoot/..
try {

    npx http-server ./Build/PackageRegistry.Local -a 127.0.0.1 -p 8000

    # Nice alternative is python, but we already require node and npm, so I
    # opted for npx. (npx is bundled with npm)
    # python -m http.server -d ./Build/PackageRegistry 8000

} finally {
    Pop-Location
}
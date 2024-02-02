## Publishing

This section pertains only to members of the MapStation team who publish new versions of MapStation to Thunderstore, Github Releases, and Github Pages.

Required tools:

- [Git for Windows](https://gitforwindows.org/)
- [Github CLI](https://cli.github.com/) for publishing
- [PowerShell Core](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.4)
- [Node and npm](https://nodejs.org/en)

Tools can be installed manually, or with winget:

```shell
winget install -e --id Git.Git
winget install -e --id GitHub.cli
winget install --id Microsoft.Powershell --source winget
winget install -e --id OpenJS.NodeJS
```

### Release Checklist

*Do these steps to publish a new version*

Ensure published MapStation.Editor has same dependencies as development editor:

```shell
./scripts/sync-package-manifests.ps1
git add MapStation.Editor/Packages/manifest*.json
git commit -m 'syncing editor dependencies'
```

Bump version number:

```shell
./scripts/version.ps1 -minor # See script for supported -flags
# Don't forget to push the git tag to github
git push
git push --tags
```

Build & zip plugin, editor, and package registry.

```shell
./scripts/package.ps1 -Release
```

*Build output goes into `Build` directory.*

Create a new "Release" on github, selecting the git tag you previously pushed. Upload plugin and editor .zips to the release.

Push the updated package registry to git, which publishes to github pages. (static file hosting)

*This is a different `git clone` and will push to a different git repository*

```shell
cd ./Build/PackageRegistry.Publish
git add *
git commit -m 'publish new version'
git push
```

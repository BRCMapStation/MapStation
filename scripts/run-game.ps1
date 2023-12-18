$bepinexDirectory = $env:BepInExDirectory
$preloaderPath = "$bepinexDirectory\core\BepInEx.Preloader.dll"
$steam = (Get-ItemProperty HKLM:/SOFTWARE/WOW6432Node/Valve/Steam).InstallPath + "\Steam.exe"

& $steam -applaunch 1353230 --doorstop-enable true --doorstop-target "$preloaderPath"

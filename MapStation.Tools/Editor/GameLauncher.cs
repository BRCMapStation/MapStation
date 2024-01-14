
using System;
using System.Diagnostics;
using System.IO;

public class GameLauncher {
    public static void LaunchGameSteam() {
        var bepinexDirectory = Environment.GetEnvironmentVariable("BepInExDirectory", EnvironmentVariableTarget.User);
        if (bepinexDirectory == null)
            bepinexDirectory = Environment.GetEnvironmentVariable("BepInExDirectory", EnvironmentVariableTarget.Machine);
        if (bepinexDirectory == null) {
            return;
        }
        var preloaderPath = Path.Combine(bepinexDirectory, "core/BepInEx.Preloader.dll");
        preloaderPath = preloaderPath.Replace("/", @"\");
        var args = $"-applaunch 1353230 --doorstop-enable true --doorstop-target \"{preloaderPath}\"";
        var steamLoc = GetSteamExecutablePath();
        if (string.IsNullOrEmpty(steamLoc))
            return;
        Process.Start(steamLoc, args);
    }

    private static string GetSteamExecutablePath() {
        var installPath = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam", "InstallPath", "") as string;
        return Path.Combine(installPath, "steam.exe");
    }

    public static bool IsGameOpen()
    {
        if (Process.GetProcessesByName("Bomb Rush Cyberfunk").Length > 0)
            return true;
        return false;
    }

    public static bool CanLaunchOnSteam() {
        var steamLoc = GameLauncher.GetSteamExecutablePath();
        return !string.IsNullOrEmpty(steamLoc);
    }
}
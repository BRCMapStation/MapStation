using System.Diagnostics;
using System.IO;

namespace MapStation.Tools.DevTools {
    public class PowershellUtil {
        public static Process RunScript(string script) {
            var startInfo = new ProcessStartInfo();
            var powershellDirectory = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\1\PowerShellEngine", "ApplicationBase", "") as string;
            startInfo.FileName = Path.Combine(powershellDirectory, "powershell.exe");
            startInfo.WorkingDirectory = Path.GetDirectoryName(script);
            script = "\"&'" + script + "'\"";
            startInfo.Arguments = script;
            return Process.Start(startInfo);
        }
    }
}

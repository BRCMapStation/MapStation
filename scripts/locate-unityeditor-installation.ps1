Locate Unity Editor install

%programdata%\Microsoft\Windows\Start Menu\Programs\Unity 2021.3.27f1
%appdata%\Microsoft\Windows\Start Menu\Programs\Unity 2021.3.27f1

Get-process, find unity.exe running.

C:\Users\cspot\AppData\Local\Unity\Editor

$sh = New-Object -COM WScript.Shell
$target = $sh.CreateShortcut('<path>').Target
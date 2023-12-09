# Locate Unity Editor install

function findFromStartMenu($lnk) {
    if(Test-Path $lnk) {
        $sh = New-Object -COM WScript.Shell
        $target = $sh.CreateShortcut($lnk).TargetPath
        Write-Host "Found Unity Editor executable from Start Menu shortcut: $target"
    }
}

function findFromRunningProcess() {
    $runningEditor = Get-Process | Where-Object {
        $_.Path -like '*\Unity.exe'
    }
    if($runningEditor) {
        Write-Host "Found Unity Editor executable by checking currently running processes: $($runningEditor.Path)"
    }
}

findFromStartMenu "$($env:programdata)\Microsoft\Windows\Start Menu\Programs\Unity 2021.3.27f1\Unity.lnk"
findFromStartMenu "$($env:appdata)\Microsoft\Windows\Start Menu\Programs\Unity 2021.3.27f1\Unity.lnk"

findFromRunningProcess

Read-Host -Prompt "Press enter to exit"

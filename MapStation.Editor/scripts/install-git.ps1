Write-Output "====== Installing Software ======="
try
{
    git | Out-Null
   "Git is already installed"
}
catch [System.Management.Automation.CommandNotFoundException]
{
    winget install --id Git.Git -e --source winget
}
Read-Host "Press enter to close"

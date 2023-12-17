param(
    [switch]$restart
)

# ssh key WinterlandSlopCrew.pem should be stored at ./scripts/credentials/WinterlandSlopCrew.pem
# Get it from cspotcode

if($restart) {
    ssh -i $PSScriptRoot/credentials/WinterlandSlopCrew.pem ubuntu@winterland.cspotcode.com 'cd Winterland && ./run-slopcrew.ps1'
} else {
    ssh -i $PSScriptRoot/credentials/WinterlandSlopCrew.pem ubuntu@winterland.cspotcode.com
}

# Once you're in, you'll be in a bash shell.  `pwsh` (powershell) is installed if you prefer that.
# 
# `cd Winterland` cuz everything is in there.
#
# Two scripts start everything:
#   ./run-slopcrew.ps1
#   ./run-slopnet.ps1
#
# SlopNet runs inside a "screen" session, which is like an interactive terminal
# that runs in the background that you can attach to.
#
# To attach to the screen session, run `screen -R`
# To detach, press Ctrl+a then d
#
# https://askubuntu.com/questions/124897/how-do-i-detach-a-screen-session-from-a-terminal
#

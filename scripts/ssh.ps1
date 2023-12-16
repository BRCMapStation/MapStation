# ssh key WinterlandSlopCrew.pem should be stored at ./scripts/credentials/WinterlandSlopCrew.pem
# Get it from cspotcode

ssh -i $PSScriptRoot/credentials/WinterlandSlopCrew.pem ubuntu@winterland.cspotcode.com

# Once you're in, you'll be in a bash shell.  `pwsh` (powershell) is installed if you prefer
# All Winterland code and server is stored in `Winterland`

# To pull latest server code and restart the server:
#
#     cd Winterland
#     ./update-server.ps1



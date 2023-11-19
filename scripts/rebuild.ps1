cd ../

Write-Output "====== Building Common Scripts ======="

cd Winterland.Common
dotnet build

Write-Output "====== Building Plugin ======="

cd ../
cd Winterland.Plugin
dotnet build

cd ../
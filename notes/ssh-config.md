*Very* messy notes when cspotcode created a SlopCrew server on EC2

How I configured the server:

https://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu#im-using-ubuntu-2204-or-later-and-i-only-need-net

```bash
# Get Ubuntu version
declare repo_version=$(if command -v lsb_release &> /dev/null; then lsb_release -r -s; else grep -oP '(?<=^VERSION_ID=).+' /etc/os-release | tr -d '"'; fi)

# Download Microsoft signing key and repository
wget https://packages.microsoft.com/config/ubuntu/$repo_version/packages-microsoft-prod.deb -O packages-microsoft-prod.deb

# Install Microsoft signing key and repository
sudo dpkg -i packages-microsoft-prod.deb

# Clean up
rm packages-microsoft-prod.deb

# Update packages
sudo apt update
```

Also deprioritized the ubuntu sources so everything installed from MS:

https://learn.microsoft.com/en-us/dotnet/core/install/linux-package-mixup?pivots=os-linux-ubuntu#i-need-a-version-of-net-that-isnt-provided-by-my-linux-distribution

My `/etc/apt/preferences`
```
Package: dotnet* aspnet* netstandard*
Pin: origin "archive.ubuntu.com"
Pin-Priority: -10

Package: dotnet* aspnet* netstandard*
Pin: origin "security.ubuntu.com"
Pin-Priority: -10

Package: dotnet* aspnet* netstandard*
Pin: origin "us-east-1.ec2.archive.ubuntu.com"
Pin-Priority: -10
```

```
sudo apt install dotnet-runtime-7.0
sudo apt install dotnet-sdk-8.0
sudo apt install powershell
```

# Installing GameNetworkingSockets

```
sudo apt install libssl-dev
sudo apt install libprotobuf-dev protobuf-compiler
sudo apt-get install pkg-config
cd ~/Winterland
git clone https://github.com/microsoft/vcpkg/
./vcpkg/bootstrap-vcpkg.sh
```

Then from SlopCrew code:

```
cd ~/Winterland/SlopCrew
../vcpkg/vcpkg install
```

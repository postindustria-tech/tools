param(
    [string]$NugetRepo = "https://nuget.pkg.github.com/51Degrees/index.json",
    [Parameter(Mandatory)][string]$NugetUser,
    [Parameter(Mandatory)][string]$NugetPassword
)
$ErrorActionPreference = "Stop"
$PSNativeCommandUseErrorActionPreference = $true

dotnet nuget add source $NugetRepo --name 51Degrees --username $NugetUser --password $NugetPassword --store-password-in-clear-text

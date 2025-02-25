param (
    [Parameter(Mandatory)][string]$RepoName,
    [Parameter(Mandatory)][string]$DataFile
)
$ErrorActionPreference = "Stop"
$PSNativeCommandUseErrorActionPreference = $true

Push-Location $RepoName/PropertyGenerator
try {
    dotnet build -c Release
    dotnet run $DataFile ..
} finally {
    Pop-Location
}

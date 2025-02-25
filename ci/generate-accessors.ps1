param (
    [Parameter(Mandatory)][string]$RepoName,
    [string]$DataFile = "$RepoName/TAC-HashV41.hash"
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

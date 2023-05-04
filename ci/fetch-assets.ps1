
param (
    [string]$RepoName,
    [Parameter(Mandatory=$true)]
    [string]$DeviceDetection,
    [string]$DeviceDetectionUrl
)

# Fetch the TAC data file for building with
./steps/fetch-hash-assets.ps1 -RepoName $RepoName -LicenseKey $DeviceDetection -Url $DeviceDetectionUrl
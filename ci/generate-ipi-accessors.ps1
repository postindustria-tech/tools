param (
    [string]$RepoName,
    [string]$TargetRepo
)

$RepoPath = [IO.Path]::Combine($pwd, $RepoName)
$GeneratorPath = [IO.Path]::Combine($RepoPath, "PropertyGenerator")
$DataFile = [IO.Path]::Combine($RepoPath, "51Degrees-EnterpriseV4.ipi")

Push-Location $GeneratorPath

try {

    dotnet build -c Release
    dotnet run $DataFile $RepoPath

    if ($TargetRepo -eq "ip-intelligence-dotnet") {
        Copy-Item "$RepoPath/CSharp/IIpIntelligenceData.cs" "../../$TargetRepo/FiftyOne.IpIntelligence.Data/Data/"
        Copy-Item "$RepoPath/CSharp/IpIntelligenceDataBase.cs" "../../$TargetRepo/FiftyOne.IpIntelligence.Data/"
    }

}
finally {
    Pop-Location
}

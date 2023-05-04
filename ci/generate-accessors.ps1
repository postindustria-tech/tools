param (
    [string]$RepoName,
    [string]$TargetRepo
)

$RepoPath = [IO.Path]::Combine($pwd, $RepoName)
$GeneratorPath = [IO.Path]::Combine($RepoPath, "PropertyGenerator")
$DataFile = [IO.Path]::Combine($RepoPath, "TAC-HashV41.hash")

Push-Location $GeneratorPath

try {

    dotnet build -c Release
    dotnet run $DataFile $RepoPath

    if ($TargetRepo -eq "device-detection-java") {
        Copy-Item "$RepoPath/Java/DeviceDataBase.java" "../../$TargetRepo/device-detection.shared/src/main/java/fiftyone/devicedetection/shared/"
        Copy-Item "$RepoPath/Java/DeviceData.java" "../../$TargetRepo/device-detection.shared/src/main/java/fiftyone/devicedetection/shared/"
    }
    elseif ($TargetRepo -eq "device-detection-dotnet") {
        Copy-Item "$RepoPath/CSharp/IDeviceData.cs" "../../$TargetRepo/FiftyOne.DeviceDetection/FiftyOne.DeviceDetection.Data/Data/"
        Copy-Item "$RepoPath/CSharp/DeviceDataBase.cs" "../../$TargetRepo/FiftyOne.DeviceDetection/FiftyOne.DeviceDetection.Data/"
    }

}
finally {
    Pop-Location
}
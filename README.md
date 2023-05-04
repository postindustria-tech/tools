# V4 Pipeline Tools

##Property Generation Tool
The `PropertyGenerationTool` generates code for `DeviceDataBase` and `DeviceData` (`IDeviceData` in .NET) classes for
device detection Java and .NET using the V4 data file.

See: 
[DeviceDataBase.java](https://github.com/51Degrees/device-detection-java/blob/master/device-detection.shared/src/main/java/fiftyone/devicedetection/shared/DeviceDataBase.java)
[DeviceData.java](https://github.com/51Degrees/device-detection-java/blob/master/device-detection.shared/src/main/java/fiftyone/devicedetection/shared/DeviceData.java)

[IDeviceData.cs](https://github.com/51Degrees/device-detection-dotnet/blob/master/FiftyOne.DeviceDetection/FiftyOne.DeviceDetection.Data/Data/IDeviceData.cs)
[DeviceDataBase.cs](https://github.com/51Degrees/device-detection-dotnet/blob/master/FiftyOne.DeviceDetection/FiftyOne.DeviceDetection.Data/DeviceDataBase.cs)

The License text is kept in the PropertyGenerationTool folder and should contain only the license text.

This is program is run as a part of a daily workflow which rebuilds the device classes from the most recent V4 TAC data file downloaded from the distributor.
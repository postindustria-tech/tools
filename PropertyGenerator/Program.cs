using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FiftyOne.DeviceDetection.Hash.Engine.OnPremise.FlowElements;
using Microsoft.Extensions.Logging;
using FiftyOne.Pipeline.Engines;

namespace PropertyGenerationTool
{
    class Program
    {
        private static ILoggerFactory LoggerFactory = new LoggerFactory();

        static void Main(string[] args)
        {
            using (var engine = new DeviceDetectionHashEngineBuilder(LoggerFactory)
                .SetAutoUpdate(false)
                .SetDataFileSystemWatcher(false)
                .SetPerformanceProfile(PerformanceProfiles.HighPerformance)
                .Build(args[0], false))
            {
                // C#.
                DeviceDetection.BuildCSharp(engine, Path.Combine(args[1], "CSharp"));
                // Java.
                DeviceDetection.BuildJava(engine, Path.Combine(args[1], "Java"));
            }
            Console.WriteLine("Done Device Detection.");

        }
    }
}

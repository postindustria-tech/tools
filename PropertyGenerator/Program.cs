using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FiftyOne.DeviceDetection.Hash.Engine.OnPremise.FlowElements;
using Microsoft.Extensions.Logging;
using FiftyOne.Pipeline.Engines;
using PropertyGenerator;
using FiftyOne.IpIntelligence.Engine.OnPremise.FlowElements;

namespace PropertyGenerationTool
{
    class Program
    {
        private static ILoggerFactory LoggerFactory = new LoggerFactory();

        static void Main(string[] args)
        {
            if (args[0].EndsWith(".hash"))
            {
                using (var engine = new DeviceDetectionHashEngineBuilder(LoggerFactory)
                    .SetAutoUpdate(false)
                    .SetDataFileSystemWatcher(false)
                    .SetPerformanceProfile(PerformanceProfiles.HighPerformance)
                    .Build(args[0], false))
                {
                    var deviceDetection = new DeviceDetection(engine);
                    // C#.
                    deviceDetection.BuildCSharp(Path.Combine(args[1], "CSharp"));
                    // Java.
                    deviceDetection.BuildJava(Path.Combine(args[1], "Java"));
                }
                Console.WriteLine("Done Device Detection.");
            }
            else if (args[0].EndsWith(".ipi"))
            {
                using (var engine = new IpiOnPremiseEngineBuilder(LoggerFactory)
                    .SetAutoUpdate(false)
                    .SetDataFileSystemWatcher(false)
                    .SetDataUpdateOnStartup(false)
                    .SetPerformanceProfile(PerformanceProfiles.HighPerformance)
                    .Build(args[0], false))
                {
                    var ipIntelligence = new IpIntelligence(engine);
                    // C#
                    ipIntelligence.BuildCSharp(Path.Combine(args[1], "CSharp"));
                    // Java
                    ipIntelligence.BuildJava(Path.Combine(args[1], "Java"));
                    Console.WriteLine("Done IP Intelligence");
                }
            }
        }
    }
}

using FiftyOne.DeviceDetection.Hash.Engine.OnPremise.FlowElements;
using FiftyOne.Pipeline.Core.Data.Types;
using FiftyOne.Pipeline.Engines.FiftyOne.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PropertyGenerationTool
{
    internal class CSClassBuilder
    {
        internal static string GetReturnType(IFiftyOneAspectPropertyMetaData property)
        {
            string typeString;
            switch (property.Type)
            {
                case Type intType when intType == typeof(Int32):
                    typeString = "int";
                    break;
                case Type boolType when boolType == typeof(Boolean):
                    typeString = "bool";
                    break;
                case Type doubleType when doubleType == typeof(Double):
                    typeString = "double";
                    break;
                case Type listType when listType == typeof(IReadOnlyList<string>):
                    typeString = "IReadOnlyList<string>";
                    break;
                case Type javaScriptType when javaScriptType == typeof(JavaScript):
                    typeString = "JavaScript";
                    break;
                case Type stringType when stringType == typeof(String):
                default:
                    typeString = "string";
                    break;
            }

            return $"IAspectPropertyValue<{typeString}>";
        }

        internal static string GetGetterName(IFiftyOneAspectPropertyMetaData property)
        {
            return property.Name
                .Replace("/", "")
                .Replace("-", "");
        }

        internal static string GetLowerName(IFiftyOneAspectPropertyMetaData property)
        {
            return property.Name.ToLower();
        }

        internal static string GetGetter(IFiftyOneAspectPropertyMetaData property)
        {
            return String.Format(
                "public {0} {1} {{ get {{ return GetAs<{0}>(\"{2}\"); }} }}",
                GetReturnType(property),
                GetGetterName(property),
                property.Name);
        }

        internal static string GetDescription(IFiftyOneAspectPropertyMetaData property)
        {
            return Regex.Replace(property.Description, @"<.*>", delegate (Match match)
            {
                return $"<![CDATA[{match.Value}]]>";
            });
        }
        internal static string GetKeyValuePair(IFiftyOneAspectPropertyMetaData property)
        {
           return String.Format("{{ \"{0}\", typeof({1}) }}",
               property.Name,
               GetReturnType(property));
        }

        internal static class Shared
        {
            
            private static string classNamespace = "FiftyOne.DeviceDetection.Shared";
            private static string interfaceNamespace = "FiftyOne.DeviceDetection";

            internal static void BuildInterface(DeviceDetectionHashEngine engine, string outputPath)
            {
                using (var outputStream = new FileStream(outputPath, FileMode.Create))
                using (var writer = new StreamWriter(outputStream))
                {
                    DeviceDetection.WriteCopyright(writer);
                    writer.WriteLine("using FiftyOne.Pipeline.Core.Data.Types;");
                    writer.WriteLine("using FiftyOne.Pipeline.Engines.Data;");
                    writer.WriteLine("using System.Collections.Generic;");
                    writer.WriteLine("");

                    writer.WriteLine("// This interface sits at the top of the name space in order to make ");
                    writer.WriteLine("// life easier for consumers.");
                    writer.WriteLine(String.Format("namespace {0}", interfaceNamespace));
                    writer.WriteLine("{");
                    writer.WriteLine("\t/// <summary>");
                    writer.WriteLine("\t/// Represents a data object containing values relating to a device.");
                    writer.WriteLine("\t/// This includes the hardware, operating system and browser as");
                    writer.WriteLine("\t/// well as crawler details if the request actually came from a ");
                    writer.WriteLine("\t/// bot or other automated system.");
	                writer.WriteLine("\t/// </summary>");
                    writer.WriteLine("\tpublic interface IDeviceData : IAspectData");
                    writer.WriteLine("\t{");
                    foreach (var property in engine.Properties
                        .Where(p => Constants.excludedProperties.Contains(p.Name) == false)
                        .OrderBy(p => p.Name))
                    {
                        writer.WriteLine("\t\t/// <summary>");
                        writer.WriteLine("\t\t/// " + GetDescription(property));
                        writer.WriteLine("\t\t/// </summary>");
                        writer.WriteLine("\t\t{0} {1} {{ get; }}",
                            GetReturnType(property),
                            GetGetterName(property));
                    }

                    writer.WriteLine("\t}");
                    writer.WriteLine("}");
                }
            }

            internal static void BuildClass(DeviceDetectionHashEngine engine, string outputPath)
            {
                using (var outputStream = new FileStream(outputPath, FileMode.Create))
                using (var writer = new StreamWriter(outputStream))
                {
                    DeviceDetection.WriteCopyright(writer);
                    writer.WriteLine("using FiftyOne.Pipeline.Core.Data;");
                    writer.WriteLine("using FiftyOne.Pipeline.Core.Data.Types;");
                    writer.WriteLine("using FiftyOne.Pipeline.Core.FlowElements;");
                    writer.WriteLine("using FiftyOne.Pipeline.Engines.Data;");
                    writer.WriteLine("using FiftyOne.Pipeline.Engines.FlowElements;");
                    writer.WriteLine("using FiftyOne.Pipeline.Engines.Services;");
                    writer.WriteLine("using Microsoft.Extensions.Logging;");
                    writer.WriteLine("using System;");
                    writer.WriteLine("using System.Collections.Generic;");
                    writer.WriteLine(String.Format("namespace {0}", classNamespace));
                    writer.WriteLine("{");
                    writer.WriteLine("\t/// <summary>");
                    writer.WriteLine("\t/// Abstract base class for properties relating to a device.");
                    writer.WriteLine("\t/// This includes the hardware, operating system and browser as");
                    writer.WriteLine("\t/// well as crawler details if the request actually came from a ");
                    writer.WriteLine("\t/// bot or other automated system.");
                    writer.WriteLine("\t/// </summary>");
                    writer.WriteLine("\tpublic abstract class DeviceDataBase : AspectDataBase, IDeviceData");
                    writer.WriteLine("\t{");
                    writer.WriteLine("\t\t/// <summary>");
                    writer.WriteLine("\t\t/// Constructor.");
                    writer.WriteLine("\t\t/// </summary>");
                    writer.WriteLine("\t\t/// <param name=\"logger\">");
                    writer.WriteLine("\t\t/// The logger for this instance to use.");
                    writer.WriteLine("\t\t/// </param>");
                    writer.WriteLine("\t\t/// <param name=\"pipeline\">");
                    writer.WriteLine("\t\t/// The Pipeline this data instance has been created by.");
                    writer.WriteLine("\t\t/// </param>");
                    writer.WriteLine("\t\t/// <param name=\"engine\">");
                    writer.WriteLine("\t\t/// The engine this data instance has been created by.");
                    writer.WriteLine("\t\t/// </param>");
                    writer.WriteLine("\t\t/// <param name=\"missingPropertyService\">");
                    writer.WriteLine("\t\t/// The missing property service to use when a requested property");
		            writer.WriteLine("\t\t/// does not exist.");
                    writer.WriteLine("\t\t/// </param>");
                    writer.WriteLine("\t\tprotected DeviceDataBase(");
                    writer.WriteLine("\t\t\tILogger<AspectDataBase> logger,");
                    writer.WriteLine("\t\t\tIPipeline pipeline,");
                    writer.WriteLine("\t\t\tIAspectEngine engine,");
                    writer.WriteLine("\t\t\tIMissingPropertyService missingPropertyService)");
                    writer.WriteLine("\t\t\t: base(logger, pipeline, engine, missingPropertyService) { }");
                    writer.WriteLine("");

                    writer.WriteLine("\t\tprotected static readonly IReadOnlyDictionary<string, Type> PropertyTypes =");
                    writer.WriteLine("\t\t\tnew Dictionary<string, Type>()");
                    writer.WriteLine("\t\t\t{");

                    List<IFiftyOneAspectPropertyMetaData> properties = engine.Properties
                        .Where(p => Constants.excludedProperties.Contains(p.Name) == false)
                        .OrderBy(p => p.Name)
                        .ToList();

                    foreach (var property in properties)
                    {
                        // Checks if the current element is the last
                        // and adds coma at the end if it is not.
                        if (properties.IndexOf(property) != properties.Count -1)
                        {
                            writer.WriteLine("\t\t\t\t" + GetKeyValuePair(property) + ",");
                        }
                        else
                        {
                            writer.WriteLine("\t\t\t\t" + GetKeyValuePair(property));
                        }
                    }
                    writer.WriteLine("\t\t\t};");
                    writer.WriteLine("");

                    foreach (var property in properties)
                    {
                        writer.WriteLine("\t\t/// <summary>");
                        writer.WriteLine("\t\t/// " + GetDescription(property));
                        writer.WriteLine("\t\t/// </summary>");
                        writer.WriteLine("\t\t" + GetGetter(property));
                    }
                    writer.WriteLine("\t}");
                    writer.WriteLine("}");
                }
            }
        }
    }
}

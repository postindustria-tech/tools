using FiftyOne.DeviceDetection.Hash.Engine.OnPremise.FlowElements;
using FiftyOne.Pipeline.Core.Data.Types;
using FiftyOne.Pipeline.Engines.FiftyOne.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyGenerationTool
{
    class JavaClassBuilder
    {
        internal static string GetType(IFiftyOneAspectPropertyMetaData property)
        {
            string typeString;
            switch (property.Type)
            {
                case Type intType when intType == typeof(Int32):
                    typeString = "Integer";
                    break;
                case Type boolType when boolType == typeof(Boolean):
                    typeString = "Boolean";
                    break;
                case Type doubleType when doubleType == typeof(Double):
                    typeString = "Double";
                    break;
                case Type listType when listType == typeof(IReadOnlyList<string>):
                    typeString = "List<String>";
                    break;
                case Type javaScriptType when javaScriptType == typeof(JavaScript):
                    typeString = "JavaScript";
                    break;
                case Type stringType when stringType == typeof(String):
                default:
                    typeString = "String";
                    break;
            }
            return $"{typeString}";
        }

        internal static string GetGetterName(IFiftyOneAspectPropertyMetaData property)
        {
            return "get" + property.Name
                .Replace("/", "")
                .Replace("-", "");
        }

        internal static string GetLowerName(IFiftyOneAspectPropertyMetaData property)
        {
            return property.Name.ToLower();
        }

        internal static string GetAsString(IFiftyOneAspectPropertyMetaData property)
        {
            return $"((String)this.getWithCheck(\"{property.Name.ToLower()}\"))";
        }

        internal static string GetGetter(IFiftyOneAspectPropertyMetaData property)
        {
            return $"public AspectPropertyValue<{GetType(property)}> {GetGetterName(property)}() {{ return getAs(\"{property.Name.ToLower()}\", AspectPropertyValue.class, {GetType(property).Split('<')[0]}.class); }}";
        }

        internal static void BuildInterface(DeviceDetectionHashEngine engine, string outputPath)
        {
            using (var outputStream = new FileStream(outputPath, FileMode.Create))
            using (var writer = new StreamWriter(outputStream))
            {
                DeviceDetection.WriteCopyright(writer);
                writer.WriteLine("package fiftyone.devicedetection.shared;");

                writer.WriteLine("import fiftyone.pipeline.core.data.types.JavaScript;");
                writer.WriteLine("import fiftyone.pipeline.engines.data.AspectData;");
                writer.WriteLine("import fiftyone.pipeline.engines.data.AspectPropertyValue;");
                writer.WriteLine("import java.util.List;");

                writer.WriteLine("// This interface sits at the top of the name space in order to make");
                writer.WriteLine("// life easier for consumers.");
                writer.WriteLine("/**");
                writer.WriteLine(" * Interface exposing typed accessors for properties related to a device");
                writer.WriteLine(" * returned by a device detection engine.");
                writer.WriteLine(" */");
                writer.WriteLine("public interface DeviceData extends AspectData");
                writer.WriteLine("{");
                foreach (var property in engine.Properties
                    .Where(p => Constants.excludedProperties.Contains(p.Name) == false)
                    .OrderBy(p => p.Name))
                {
                    writer.WriteLine("\t/**");
                    writer.WriteLine("\t * " + property.Description);
                    writer.WriteLine("\t */");
                    writer.WriteLine("\tAspectPropertyValue<{0}> {1}();",
                        GetType(property),
                        GetGetterName(property));
                }

                writer.WriteLine("}");
            }
        }

        internal static void BuildClass(DeviceDetectionHashEngine engine, string outputPath)
        {
            using (var outputStream = new FileStream(outputPath, FileMode.Create))
            using (var writer = new StreamWriter(outputStream))
            {
                DeviceDetection.WriteCopyright(writer);
                writer.WriteLine("package fiftyone.devicedetection.shared;");

                writer.WriteLine("import fiftyone.pipeline.core.data.FlowData;");
                writer.WriteLine("import fiftyone.pipeline.core.data.types.JavaScript;");
                writer.WriteLine("import fiftyone.pipeline.engines.data.AspectData;");
                writer.WriteLine("import fiftyone.pipeline.engines.data.AspectDataBase;");
                writer.WriteLine("import fiftyone.pipeline.engines.data.AspectPropertyMetaData;");
                writer.WriteLine("import fiftyone.pipeline.engines.flowelements.AspectEngine;");
                writer.WriteLine("import fiftyone.pipeline.engines.data.AspectPropertyValue;");
                writer.WriteLine("import fiftyone.pipeline.engines.services.MissingPropertyService;");
                writer.WriteLine("import org.slf4j.Logger;");
                writer.WriteLine("import java.util.List;");

                writer.WriteLine("public abstract class DeviceDataBase extends AspectDataBase implements DeviceData");
                writer.WriteLine("{");

                writer.WriteLine("/**");
                writer.WriteLine(" * Constructor.");
                writer.WriteLine(" * @param logger used for logging");
                writer.WriteLine(" * @param flowData the {@link FlowData} instance this element data will be");
                writer.WriteLine(" *                 associated with");
                writer.WriteLine(" * @param engine the engine which created the instance");
                writer.WriteLine(" * @param missingPropertyService service used to determine the reason for");
                writer.WriteLine(" *                               a property value being missing");
                writer.WriteLine(" */");
                writer.WriteLine("\tprotected DeviceDataBase(");
                writer.WriteLine("\t\tLogger logger,");
                writer.WriteLine("\t\tFlowData flowData,");
                writer.WriteLine("\t\tAspectEngine<? extends AspectData, ? extends AspectPropertyMetaData> engine,");
                writer.WriteLine("\t\tMissingPropertyService missingPropertyService) {");
                writer.WriteLine("\t\tsuper(logger, flowData, engine, missingPropertyService);");
                writer.WriteLine("\t}");

                foreach (var property in engine.Properties
                    .Where(p => Constants.excludedProperties.Contains(p.Name) == false))
                {
                    writer.WriteLine("\t/**");
                    writer.WriteLine("\t * " + property.Description);
                    writer.WriteLine("\t */");
                    writer.WriteLine("\t@SuppressWarnings(\"unchecked\")");
                    writer.WriteLine("\t@Override");
                    writer.WriteLine("\t" + GetGetter(property));
                }
                writer.WriteLine("}");
            }
        }
    }
}

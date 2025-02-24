using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyGenerationTool
{
    /// <summary>
    /// Abstract base class for buiding class files.
    /// </summary>
    /// <typeparam name="T">
    /// The type of property which is read. This can be from an engine,
    /// or metadata.
    /// </typeparam>
    internal abstract class ClassBuilderBase<T>
    {
        /// <summary>
        /// Get the name of the property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected abstract string GetPropertyName(T property);

        /// <summary>
        /// Get the type of the property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected abstract Type GetPropertyType(T property);

        /// <summary>
        /// Get the description from the property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected abstract string GetPropertyDescription(T property);
    }
}

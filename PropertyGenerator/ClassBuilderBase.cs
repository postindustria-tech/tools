using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyGenerationTool
{
    internal abstract class ClassBuilderBase<T>
    {
        protected abstract string GetPropertyName(T property);
        protected abstract Type GetPropertyType(T property);
        protected abstract string GetPropertyDescription(T property);
    }
}

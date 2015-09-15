using System;
using System.Collections.Generic;

namespace Invert.Core.GraphDesigner
{
    public class SelectTypeCommand : Command
    {

        public bool AllowNone { get; set; }
        public bool PrimitiveOnly { get; set; }
        public bool IncludePrimitives { get; set; }

        public Predicate<ITypeInfo> Filter { get; set; }
        public TypedItemViewModel ItemViewModel { get; set; }
    }
}
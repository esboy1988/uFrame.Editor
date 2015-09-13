using System;
using System.Collections.Generic;
using Invert.Json;

namespace Invert.Core.GraphDesigner
{
    public class TypeReferenceNode : GenericNode, IClassTypeNode, ITypedItem
    {
        
        private string _name1;
        private Type _type;

        public override string FullName
        {
            get { return Name; }
            set { Name = value; }
        }

        [JsonProperty]
        public override string Name
        {
            get { return _name1 ; }
            set
            {
                _name1 = value;
            }
        }

        public string ClassName
        {
            get { return Name; }
            set { Name = value; }
        }

        public Type Type
        {
            get { return _type ?? (_type = InvertApplication.FindTypeByName(Name)); }
            set { _type = value; }
        }

        public override IEnumerable<IMemberInfo> GetMembers()
        {
            foreach (var item in new SystemTypeInfo(Type).GetMembers())
            {
                yield return item;
            }
        }

        public override void Validate(List<ErrorInfo> errors)
        {
            base.Validate(errors);
           
        }
    }
}
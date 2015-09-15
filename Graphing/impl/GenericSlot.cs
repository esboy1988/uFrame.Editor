using System.Collections.Generic;
using System.Linq;
using Invert.Data;

namespace Invert.Core.GraphDesigner
{
    public class GenericSlot : GenericNodeChildItem
    {
        
        public virtual bool Validate(IDiagramNodeItem a, IDiagramNodeItem b)
        {
            
            return a != b;
        }

        public virtual void SetInput(IValueItem item)
        {
            if (!AllowMultipleInputs || item == null)
            {
                foreach (var input in Inputs)
                    Repository.Remove(input);

            }
            if (!AllowMultipleOutputs || item == null)
            {
                foreach (var output in Outputs)
                    Repository.Remove(output);
            }
            if (item == null) return;
            
            var cd = Repository.Create<ConnectionData>();
            cd.InputIdentifier = Identifier;
            cd.OutputIdentifier = item.Identifier;
        }
        public virtual void SetOutput(IValueItem item)
        {
            if (!AllowMultipleInputs)
            {
                foreach (var input in Inputs)
                    Repository.Remove(input);

            }
            if (!AllowMultipleOutputs)
            {
                foreach (var output in Outputs)
                    Repository.Remove(output);
            }


            var cd = Repository.Create<ConnectionData>();
            cd.InputIdentifier = item.Identifier;
            cd.OutputIdentifier = Identifier;
        }
        public virtual IEnumerable<IValueItem> GetAllowed()
        {
            return Repository.AllOf<IDataRecord>().Cast<IValueItem>();
        }

        public virtual bool AllowSelection
        {
            get { return false; }
        }

        public virtual string SelectedDisplayName
        {
            get
            {  
                var source = this.InputFrom<IDiagramNodeItem>();
                if (source != null)
                {
                    return source.Name;
                }
                return "...";
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Invert.Core.GraphDesigner
{
    public interface IConnectable : IGraphItem
    {
       
        IGraphData Graph { get; }
        IEnumerable<ConnectionData> Inputs { get; }
        IEnumerable<ConnectionData> Outputs { get; }

        bool AllowInputs { get; }
        bool AllowOutputs { get; }

        bool AllowMultipleInputs { get; }
        bool AllowMultipleOutputs { get; }
        Color Color { get; }

        //void OnConnectionApplied(IConnectable output, IConnectable input);
        bool CanOutputTo(IConnectable input);
        bool CanInputFrom(IConnectable output);

        void OnOutputConnectionRemoved(IConnectable input);
        void OnInputConnectionRemoved(IConnectable output);
        void OnConnectedToInput(IConnectable input);
        void OnConnectedFromOutput(IConnectable output);

        TType InputFrom<TType>();
        IEnumerable<TType> InputsFrom<TType>();
        IEnumerable<TType> OutputsTo<TType>();
        TType OutputTo<TType>();

    }
}
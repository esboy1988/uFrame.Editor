﻿using Invert.Data;
using Invert.Json;
using UnityEngine;

namespace Invert.Core.GraphDesigner
{
    public class FilterItem : IDataRecordRemoved, IFilterItem
    {
        private bool _collapsed;
        private string _nodeId;
        private string _filterId;
        private Vector2 _position;
        public IRepository Repository { get; set; }

        [JsonProperty]
        public string Identifier { get; set; }

        public bool Changed { get; set; }

        [JsonProperty]
        public bool Collapsed
        {
            get { return _collapsed; }
            set {
              
                this.Changed("Collapsed",ref _collapsed, value);
            }
        }

        [JsonProperty]
        public string NodeId
        {
            get { return _nodeId; }
            set {
                this.Changed("NodeId",ref _nodeId, value);
            }
        }

        [JsonProperty]
        public string FilterId
        {
            get { return _filterId; }
            set {
                this.Changed("FilterId", ref _filterId, value);
            }
        }

        public IDiagramNode Node
        {
            get
            {
                return Repository.GetById<IDiagramNode>(NodeId);
            }
        }
        public IGraphFilter Filter
        {
            get
            {
                return Repository.GetById<IGraphFilter>(FilterId);
            }
        }

        [JsonProperty]
        public Vector2 Position
        {
            get { return _position; }
            set {
               
                _position = value;
                Changed = true;
            }
        }

        public void RecordRemoved(IDataRecord record)
        {
            if (NodeId == record.Identifier || FilterId == record.Identifier)
                Repository.Remove(this);
            
        }
    }
}
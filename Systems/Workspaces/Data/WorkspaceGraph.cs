﻿using System.Collections.Generic;
using Invert.Data;
using Invert.Json;

namespace Invert.Core.GraphDesigner
{
    public class WorkspaceGraph : IDataRecord, IDataRecordRemoved
    {
        private string _workspaceId;
        private string _graphId;

        [JsonProperty]
        public string GraphId
        {
            get { return _graphId; }
            set {
                this.Changed("GraphId",ref _graphId, value);
            }
        }

        [JsonProperty]
        public string WorkspaceId
        {
            get { return _workspaceId; }
            set {
                this.Changed("WorkspaceId", ref _workspaceId, value);
            }
        }

        public IRepository Repository { get; set; }

        [JsonProperty]
        public string Identifier { get; set; }

        public bool Changed { get; set; }

        public IEnumerable<string> ForeignKeys
        {
            get
            {
                yield return GraphId;
                yield return WorkspaceId;
            }
        }

        public void RecordRemoved(IDataRecord record)
        {
            if (record.Identifier == GraphId || record.Identifier == WorkspaceId)
                Repository.Remove(this);
            
        }
    }
}
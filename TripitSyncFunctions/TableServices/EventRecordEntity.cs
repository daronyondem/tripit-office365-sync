using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace TripitSyncFunctions.TableServices
{
    public class EventRecordEntity : TableEntity
    {
        public string Hash { get; set; }
        public string GraphId { get; set; }
        public string TripItUId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrestoCore.BusinessLogic.Attributes;

namespace PrestoCore.BusinessLogic.BusinessEntities
{
    /// <summary>
    /// 
    /// </summary>
    [TableName( "TaskType" )]
    public class TaskType
    {
        /// <summary>
        /// 
        /// </summary>
        // ToDo: Fix this:
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" ), ColumnName( "TaskTypeId", true )]
        public int TaskTypeId { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "Description" )]
        public string Description { get; set; }
    }
}

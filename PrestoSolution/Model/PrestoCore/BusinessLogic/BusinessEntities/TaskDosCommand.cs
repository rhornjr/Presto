using System;
using PrestoCore.BusinessLogic.Attributes;

namespace PrestoCore.BusinessLogic.BusinessEntities
{
    /// <summary>
    /// 
    /// </summary>
    [TableName( "TaskDosCommand" )]
    [Serializable]
    public class TaskDosCommand : TaskBase
    {
        #region [Public Properties]

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "DosExecutable" )]
        public string DosExecutable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "Parameters" )]
        public string Parameters { get; set; }

        #endregion
    }
}

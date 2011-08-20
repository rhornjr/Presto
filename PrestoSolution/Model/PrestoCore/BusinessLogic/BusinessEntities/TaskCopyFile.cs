using System;
using PrestoCore.BusinessLogic.Attributes;

namespace PrestoCore.BusinessLogic.BusinessEntities
{
    /// <summary>
    /// 
    /// </summary>
    [TableName( "TaskCopyFile" )]
    [Serializable]
    public class TaskCopyFile : TaskBase
    {
        #region [Public Properties]

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "SourcePath" )]
        public string SourcePath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "SourceFileName" )]
        public string SourceFileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "DestinationPath" )]
        public string DestinationPath { get; set; }

        #endregion
    }
}

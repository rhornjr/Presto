using PrestoCore.BusinessLogic.Attributes;

namespace PrestoCore.BusinessLogic.BusinessEntities
{
    /// <summary>
    /// 
    /// </summary>
    [TableName( "TaskGroup" )]
    public class TaskGroup
    {
        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "TaskGroupId", true )]
        public int? TaskGroupId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "TaskGroupName" )]
        public string TaskGroupName { get; set; }
    }
}

using System;
using PrestoCore.BusinessLogic.Attributes;
using System.Xml.Serialization;

namespace PrestoCore.BusinessLogic.BusinessEntities
{
    /// <summary>
    /// 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes" ), TableName( "TaskItem" )]
	[Serializable]
    [XmlInclude( typeof( TaskMsi        ) )]
    [XmlInclude( typeof( TaskCopyFile   ) )]
    [XmlInclude( typeof( TaskDosCommand ) )]
    [XmlInclude( typeof( TaskXmlModify  ) )]
    public /* abstract */ class TaskBase : IComparable<TaskBase>
    {
        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "TaskItemId", true )]
        public int? TaskItemId { get; /* internal */ set; }  // ToDo: Had to comment "internal" to get WPF two-way binding to work.

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "FailureCausesAllStop" )]
        public byte FailureCausesAllStop { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "Description" )]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "TaskGroupId" )]
        public int TaskGroupId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "TaskTypeId" )]
        public int TaskTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "Sequence" )]
        public int Sequence { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool TaskSucceeded { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TaskBase()
        {
            // ToDo: Remove this. Did it to pass code analysis.
            this.TaskItemId = null;
        }

        #region IComparable<TaskBase> Members

        /// <summary>
        /// Default sort TaskBase objects on its Sequence property
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo( TaskBase other )
        {
            return this.Sequence.CompareTo( other.Sequence );
        }

        #endregion
    }
}

using System;
using PrestoCore.BusinessLogic.Attributes;

namespace PrestoCore.BusinessLogic.BusinessEntities
{
    /// <summary>
    /// 
    /// </summary>
    [TableName( "TaskXmlModify" )]
    [Serializable]
    public class TaskXmlModify : TaskBase
    {
        /// <summary>
        ///
        /// </summary>
        [ColumnName( "XmlPathAndFileName" )]
        public string XmlPathAndFileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "NodeToChange" )]
        public string NodeToChange { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "AttributeKey" )]
        public string AttributeKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "AttributeKeyValue" )]
        public string AttributeKeyValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "AttributeToChange" )]
        public string AttributeToChange { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "AttributeToChangeValue" )]
        public string AttributeToChangeValue { get; set; }
    }
}

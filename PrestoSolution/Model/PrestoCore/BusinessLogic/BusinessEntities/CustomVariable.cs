using PrestoCore.BusinessLogic.Attributes;
using System;

namespace PrestoCore.BusinessLogic.BusinessEntities
{
    /// <summary>
    /// Container for custom variables for task groups
    /// </summary>
    [TableName( "CustomVariable" )]
    [Serializable]
    public class CustomVariable : IComparable<CustomVariable>
    {
        /// <summary>
        /// ID
        /// </summary>
        [ColumnName( "CustomVariableId", true )]
        public int? CustomVariableId { get; set; }

        /// <summary>
        /// Task Group ID
        /// </summary>
        [ColumnName( "TaskGroupId", false )]
        public int TaskGroupId { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        [ColumnName( "VariableKey", false )]
        public string VariableKey { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        [ColumnName( "VariableValue", false )]
        public string VariableValue { get; set; }

        #region IComparable Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo( CustomVariable other )
        {
            return this.VariableKey.CompareTo( other.VariableKey );
        }

        #endregion
    }
}

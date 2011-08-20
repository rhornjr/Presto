using System;

namespace PrestoCore.BusinessLogic.Attributes
{
    internal sealed class ColumnNameAttribute : Attribute
    {
        internal string ColumnName   { get; set; }
        internal bool   IsPrimaryKey { get; set; }

        internal ColumnNameAttribute( string columnName )
        {
            ColumnName   = columnName;
            IsPrimaryKey = false;
        }

        internal ColumnNameAttribute( string columnName, bool isPrimaryKey )
        {
            ColumnName   = columnName;
            IsPrimaryKey = isPrimaryKey;
        }
    }
}

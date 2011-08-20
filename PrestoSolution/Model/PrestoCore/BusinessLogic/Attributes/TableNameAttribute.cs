using System;

namespace PrestoCore.BusinessLogic.Attributes
{
    internal sealed class TableNameAttribute : Attribute
    {
        internal string TableName { get; set; }

        internal TableNameAttribute( string tableName )
        {
            TableName = tableName;
        }
    }
}

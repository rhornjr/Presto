using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using PrestoCore.BusinessLogic.BusinessEntities;

namespace PrestoCore.DataAccess
{
    internal class CustomVariableDalc : DataAccessBase
    {
        private CustomVariableDalc() { }

        internal static ReadOnlyCollection<CustomVariable> GetCustomVariablesByGroupId( int groupId )
        {
            DataRow[] dataRows = prestoDataset.Tables[ "CustomVariable" ].Select( "TaskGroupId = " + groupId.ToString( CultureInfo.InvariantCulture ) );

            List<CustomVariable> customVariables = new List<CustomVariable>();

            foreach( DataRow dataRow in dataRows )
            {
                customVariables.Add( new CustomVariable()
                {
                    CustomVariableId = (int)dataRow[ "CustomVariableId" ],
                    TaskGroupId      = (int)dataRow[ "TaskGroupid"      ],
                    VariableKey      =      dataRow[ "VariableKey"      ].ToString(),
                    VariableValue    =      dataRow[ "VariableValue"    ].ToString()
                } );
            }

            return customVariables.AsReadOnly();
        }
    }
}

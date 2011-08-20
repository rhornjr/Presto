using System.Collections.Generic;
using System.Collections.ObjectModel;
using PrestoCore.BusinessLogic.BusinessEntities;
using PrestoCore.DataAccess;

namespace PrestoCore.BusinessLogic.BusinessComponents
{
    /// <summary>
    /// 
    /// </summary>
    public static class CustomVariableLogic
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customVariable"></param>
        public static void Save( CustomVariable customVariable )
        {
            Data.SaveObject<CustomVariable>( customVariable );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customVariable"></param>
        public static void Delete( CustomVariable customVariable )
        {
            Data.DeleteObject<CustomVariable>( customVariable );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate" )]
        public static ReadOnlyCollection<CustomVariable> GetAll()
        {
            return Data.GetObjectList<CustomVariable>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<CustomVariable> GetCustomVariablesByGroupId( int groupId )
        {
            List<CustomVariable> customVariables = new List<CustomVariable>( CustomVariableDalc.GetCustomVariablesByGroupId( groupId ) );
            customVariables.Sort();
            return customVariables.AsReadOnly();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CustomVariable GetObjectById( int id )
        {
            return Data.GetObjectById<CustomVariable>( id );
        }
    }
}

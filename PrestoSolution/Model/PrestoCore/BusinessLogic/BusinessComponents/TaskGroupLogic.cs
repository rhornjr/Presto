using System.Collections.ObjectModel;
using PrestoCore.BusinessLogic.BusinessEntities;
using PrestoCore.DataAccess;

namespace PrestoCore.BusinessLogic.BusinessComponents
{
    /// <summary>
    /// 
    /// </summary>
    public static class TaskGroupLogic
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskGroup"></param>
        public static void Save( TaskGroup taskGroup )
        {
            Data.SaveObject<TaskGroup>( taskGroup );
        }

        /// <summary>
        /// Deletes a <see cref="TaskGroup" /> object.
        /// </summary>
        /// <param name="taskGroup"></param>
        public static void Delete( TaskGroup taskGroup )
        {
            Data.DeleteObject<TaskGroup>( taskGroup );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate" )]
        public static ReadOnlyCollection<TaskGroup> GetAll()
        {
            return Data.GetObjectList<TaskGroup>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TaskGroup GetObjectById( int id )
        {
            return Data.GetObjectById<TaskGroup>( id );
        }
    }
}

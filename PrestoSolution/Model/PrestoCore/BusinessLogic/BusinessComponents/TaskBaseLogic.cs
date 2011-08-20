using System.Collections.ObjectModel;
using PrestoCore.BusinessLogic.BusinessEntities;
using PrestoCore.DataAccess;
using System.Collections.Generic;
using System.Security.Permissions;

namespace PrestoCore.BusinessLogic.BusinessComponents
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TaskBaseLogic
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate" )]
        public static ReadOnlyCollection<TaskBase> GetAllTaskBaseObjects()
        {
            return Data.GetObjectList<TaskBase>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TaskBase GetById( int id )
        {
            return Data.GetObjectById<TaskBase>( id );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter" )]
        public static T GetById<T>( int id )
        {
            return Data.GetObjectById<T>( id );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>Collection of <see cref="TaskBase"/> objects, sorted by Sequence.</returns>
        public static ReadOnlyCollection<TaskBase> GetTasksByGroupId( int groupId )
        {
            List<TaskBase> taskBaseList = new List<TaskBase>( TaskBaseDalc.GetTasksByGroupId( groupId ) );
            taskBaseList.Sort();
            return taskBaseList.AsReadOnly();
            //return TaskBaseDalc.GetTasksByGroupId( groupId );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskBase"></param>
        public static void Save( TaskBase taskBase )
        {
            Data.SaveObject<TaskBase>( taskBase );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskBase"></param>
        public abstract void SaveTaskBaseAsConcreteTask( TaskBase taskBase );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskItemId"></param>
        public static void DeleteTaskById( int taskItemId )
        {
            Data.DeleteTaskById( taskItemId );
        }

        /// <summary>
        /// Execute the task.
        /// </summary>
        /// <param name="task"></param>
        [SecurityPermission(SecurityAction.LinkDemand,Flags=SecurityPermissionFlag.UnmanagedCode)]
        public abstract void Execute( TaskBase task );
    }
}

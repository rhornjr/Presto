using System.Collections.ObjectModel;
using PrestoCore.BusinessLogic.BusinessEntities;
using PrestoCore.DataAccess;

namespace PrestoCore.BusinessLogic.BusinessComponents
{
    /// <summary>
    /// 
    /// </summary>
    public static class TaskTypeLogic
    {
        /// <summary>
        /// Task types need to exist in the DB. Create them if they don't exist. 
        /// </summary>
        public static void CreateAllTaskTypes()
        {
            // This is the preloaded data that must exist for Presto to work. Instead of using SQL scripts, just do this.
            // Note: It's okay if the TaskTypes already exist before doing this. If they exist, an update will happen,
            //       which just updates the data with what already exists.

            TaskTypeLogic.Save( new TaskType() { TaskTypeId = 1, Description = "TaskDosCommand" } );
            TaskTypeLogic.Save( new TaskType() { TaskTypeId = 2, Description = "TaskXmlModify"  } );
            TaskTypeLogic.Save( new TaskType() { TaskTypeId = 3, Description = "TaskCopyFile"   } );
            TaskTypeLogic.Save( new TaskType() { TaskTypeId = 4, Description = "TaskMsi"        } );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskType"></param>
        public static void Save( TaskType taskType )
        {
            Data.SaveObject<TaskType>( taskType );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TaskType GetById( int id )
        {
            return Data.GetObjectById<TaskType>( id );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate" )]
        public static ReadOnlyCollection<TaskType> GetAll()
        {
            return Data.GetObjectList<TaskType>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskType"></param>
        public static void Delete( TaskType taskType )
        {
            Data.DeleteObject<TaskType>( taskType );
        }
    }
}

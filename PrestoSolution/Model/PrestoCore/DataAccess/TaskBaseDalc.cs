using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using PrestoCore.BusinessLogic.BusinessEntities;

namespace PrestoCore.DataAccess
{
    internal class TaskBaseDalc : DataAccessBase
    {
        private TaskBaseDalc() { }

        internal static ReadOnlyCollection<TaskBase> GetTasksByGroupId( int groupId )
        {
            DataRow[] dataRows = prestoDataset.Tables[ "TaskItem" ].Select( "TaskGroupId = " + groupId.ToString( CultureInfo.InvariantCulture ) );

            List<TaskBase> tasks = new List<TaskBase>();

            foreach( DataRow dataRow in dataRows )
            {
                tasks.Add( GetConcreteTask( dataRow ) );
                //tasks.Add( new TaskBase() { TaskItemId           =  (int)dataRow[ "TaskItemId"  ],
                //                            Description          =       dataRow[ "Description" ].ToString(),
                //                            Sequence             =  (int)dataRow[ "Sequence"    ],
                //                            FailureCausesAllStop = (byte)dataRow[ "FailureCausesAllStop" ],
                //                            TaskGroupId          =  (int)dataRow[ "TaskGroupid"          ],
                //                            TaskTypeId           =  (int)dataRow[ "TaskTypeId"           ]
                //                          } );
            }

            return tasks.AsReadOnly();
        }

        private static TaskBase GetConcreteTask( DataRow dataRow )
        {
            TaskType taskType = Data.GetObjectById<TaskType>( (int)dataRow[ "TaskTypeId" ] );

            // If we ever complete Data.GetTaskBaseById(), then the switch statement below can be replaced with
            // these two lines.
            //string assemblyQualifiedClassName = "PrestoCore.BusinessLogic.BusinessEntities." + taskType.Description + ", PrestoCore";
            //return Data.GetTaskBaseById( (int)dataRow[ "TaskItemId" ], assemblyQualifiedClassName );

            switch( taskType.Description )
            {
                case "TaskMsi":
                    return Data.GetObjectById<TaskMsi>( (int)dataRow[ "TaskItemId" ] ) as TaskBase;
                case "TaskCopyFile":
                    return Data.GetObjectById<TaskCopyFile>( (int)dataRow[ "TaskItemId" ] ) as TaskBase;
                case "TaskXmlModify":
                    return Data.GetObjectById<TaskXmlModify>( (int)dataRow[ "TaskItemId" ] ) as TaskBase;
                case "TaskDosCommand":
                    return Data.GetObjectById<TaskDosCommand>( (int)dataRow[ "TaskItemId" ] ) as TaskBase;
                default:
                    return null;
            }
        }
    }
}

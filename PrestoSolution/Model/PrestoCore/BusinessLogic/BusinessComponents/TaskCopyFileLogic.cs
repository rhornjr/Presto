using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Permissions;
using PrestoCore.BusinessLogic.BusinessEntities;
using PrestoCore.DataAccess;

namespace PrestoCore.BusinessLogic.BusinessComponents
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskCopyFileLogic : TaskBaseLogic
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskCopyFile"></param>
        public static void Save( TaskCopyFile taskCopyFile )
        {
            Data.SaveObject<TaskCopyFile>( taskCopyFile );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate" )]
        public static ReadOnlyCollection<TaskCopyFile> GetAll()
        {
            return Data.GetObjectList<TaskCopyFile>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TaskCopyFile GetObjectById( int id )
        {
            return Data.GetObjectById<TaskCopyFile>( id );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        [SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" ),
         SecurityPermission( SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode )]
        public override void Execute( TaskBase task )
        {            
            TaskCopyFile taskCopyFile = task as TaskCopyFile;

            string sourcePath      = taskCopyFile.SourcePath;
            string sourceFileName  = taskCopyFile.SourceFileName;
            string destinationPath = taskCopyFile.DestinationPath;
            string status          = "Succeeded";

            try
            {
                sourcePath      = Utility.ReplaceVariablesWithValues( sourcePath,      task.TaskGroupId );
                sourceFileName  = Utility.ReplaceVariablesWithValues( sourceFileName,  task.TaskGroupId );
                destinationPath = Utility.ReplaceVariablesWithValues( destinationPath, task.TaskGroupId );

                List<string> listOfFilesToCopy = new List<string>();

                listOfFilesToCopy.AddRange( Directory.GetFiles( sourcePath, sourceFileName ) );  // Supports wildcards
            
                string fileNameOnly = string.Empty;
                
                foreach( string fileToCopy in listOfFilesToCopy )
                {
                    fileNameOnly = fileToCopy.Substring( fileToCopy.LastIndexOf( @"\" ) + 1 );  // Get just the file name
                    File.Copy( fileToCopy, destinationPath + @"\" + fileNameOnly, true );
                }

                taskCopyFile.TaskSucceeded = true;
            }
            catch( Exception ex )
            {
                taskCopyFile.TaskSucceeded = false;
                status                     = ex.Message;
                Utility.ProcessException( "Error trying to perform a Copy File task.", ex, true );
            }
            finally
            {
                Utility.Log( "Copy File\r\n" +
                             "Task Desc  : " + task.Description + "\r\n" +
                             "TaskID     : " + task.TaskItemId + "\r\n" +
                             "Source     : " + sourcePath + @"\" + sourceFileName + "\r\n" +
                             "Destination: " + destinationPath + "\r\n" +
                             "Result     : " + status );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskBase"></param>
        public override void SaveTaskBaseAsConcreteTask( TaskBase taskBase )
        {
            Data.SaveObject<TaskCopyFile>( taskBase as TaskCopyFile );
        }
    }
}

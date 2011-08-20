using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Security.Permissions;
using PrestoCore.BusinessLogic.BusinessEntities;
using PrestoCore.DataAccess;

namespace PrestoCore.BusinessLogic.BusinessComponents
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskDosCommandLogic : TaskBaseLogic
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskDosCommand"></param>
        public static void Save( TaskDosCommand taskDosCommand )
        {
            Data.SaveObject<TaskDosCommand>( taskDosCommand );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate" )]
        public static ReadOnlyCollection<TaskDosCommand> GetAll()
        {
            return Data.GetObjectList<TaskDosCommand>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TaskDosCommand GetObjectById( int id )
        {
            return Data.GetObjectById<TaskDosCommand>( id );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "processOutput" ), SecurityPermission( SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode )]
        public override void Execute( TaskBase task )
        {
            TaskDosCommand taskDosCommand = task as TaskDosCommand;

            Process process       = new Process();
            string  processOutput = string.Empty;

            try
            {
                // Hack: xcopy won't work unless you also redirect standard input. See 
                // http://www.tek-tips.com/viewthread.cfm?qid=1421150&page=12:
                // "Someone mentioned a quirk of xcopy.exe on one of the MSDN forums. When we redirect
                // output we have to redirect input too. If we don’t, it immediately and silently quits
                // right after startup."

                process.StartInfo.FileName               = Utility.ReplaceVariablesWithValues( taskDosCommand.DosExecutable, task.TaskGroupId );
                process.StartInfo.Arguments              = Utility.ReplaceVariablesWithValues( taskDosCommand.Parameters, task.TaskGroupId );
                process.StartInfo.UseShellExecute        = false;
                process.StartInfo.RedirectStandardError  = true;
                process.StartInfo.RedirectStandardInput  = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                processOutput = process.StandardOutput.ReadToEnd();

                process.WaitForExit();

                if( process.ExitCode != 0 )
                {
                    //throw new Exception( string.Format( "TaskDOSCommand failed with exit code {0}.", process.ExitCode.ToString() ) );
                }

                taskDosCommand.TaskSucceeded = true;
            }
            catch( Exception ex )
            {
                taskDosCommand.TaskSucceeded = false;
                Utility.ProcessException( "Error trying to perform a DOS command task: " + taskDosCommand.Description, ex, true );
            }
            finally
            {
                Utility.Log( string.Format( CultureInfo.CurrentCulture,
                                            "Task ID         : {0}\r\n" +
                                            "Task Description: {1}\r\n" +
                                            "Command         : {2} {3}\r\n" +
                                            "Process Output  : {4}",
                                            taskDosCommand.TaskItemId, taskDosCommand.Description, process.StartInfo.FileName,
                                            process.StartInfo.Arguments, processOutput ) );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskBase"></param>
        public override void SaveTaskBaseAsConcreteTask( TaskBase taskBase )
        {
            Data.SaveObject<TaskDosCommand>( taskBase as TaskDosCommand );
        }
    }
}

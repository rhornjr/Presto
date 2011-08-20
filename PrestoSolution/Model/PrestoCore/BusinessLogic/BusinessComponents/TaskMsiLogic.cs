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
    public class TaskMsiLogic : TaskBaseLogic
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskMsi"></param>
        public static void Save( TaskMsi taskMsi )
        {
            Data.SaveObject<TaskMsi>( taskMsi );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskBase"></param>
        public override void SaveTaskBaseAsConcreteTask( TaskBase taskBase )
        {
            Data.SaveObject<TaskMsi>( taskBase as TaskMsi );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate" )]
        public static ReadOnlyCollection<TaskMsi> GetAll()
        {
            return Data.GetObjectList<TaskMsi>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TaskMsi GetObjectById( int id )
        {
            return Data.GetObjectById<TaskMsi>( id );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes" ), SecurityPermission( SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode )]
        public override void Execute( TaskBase task )
        {
            TaskMsi taskMsi = task as TaskMsi;

            try
            {                
                Process process = new Process();
                string  arguments;

                process.StartInfo.FileName = "msiexec";
                string targetSiteParameter = taskMsi.IisWebSite.Length > 0 ? " TARGETSITE=" + taskMsi.IisWebSite : string.Empty;
                string passiveParameter    = taskMsi.PassiveInstall == 1 ? " /passive" : string.Empty;
                string targetDirParameter  = taskMsi.InstallationLocation.Length > 0 ? " TARGETDIR=\"" + taskMsi.InstallationLocation + "\"" : string.Empty;

                if( taskMsi.Install == 1 )
                {
                    // Install
                    arguments = "/i " + taskMsi.Path + "\\" + taskMsi.FileName + targetSiteParameter + passiveParameter + targetDirParameter;
                }
                else
                {
                    // Uninstall
                    arguments = "/uninstall " + taskMsi.ProductGuid + passiveParameter;
                }

                process.StartInfo.Arguments = Utility.ReplaceVariablesWithValues( arguments, task.TaskGroupId );
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;

                process.Start();

                string processOutput = process.StandardOutput.ReadToEnd();

                process.WaitForExit();

                Utility.Log( string.Format( CultureInfo.CurrentCulture,
                                            "Task Description: {0}\r\n" +
                                            "Command         : {1} {2}\r\n" +
                                            "Process Output  : {3}",
                                            taskMsi.Description, process.StartInfo.FileName, process.StartInfo.Arguments, processOutput ) );

                if( process.ExitCode != 0 )
                {
                    throw new Exception( string.Format( CultureInfo.CurrentCulture, "TaskMSI failed with an exit code of {0}.",
                                                        process.ExitCode.ToString( CultureInfo.CurrentCulture ) ) );
                }

                taskMsi.TaskSucceeded = true;
            }
            catch( Exception ex )
            {
                taskMsi.TaskSucceeded = false;
                Utility.ProcessException( string.Empty, ex, true );
            }
            finally
            {
                Utility.Log( "Task MSI\r\n" +
                             "Task Desc    : " + task.Description             + "\r\n" +
                             "TaskID       : " + task.TaskItemId              + "\r\n" +
                             "Product GUID : " + taskMsi.ProductGuid          + "\r\n" +
                             "Path         : " + taskMsi.Path                 + "\r\n" +
                             "Install Loc  : " + taskMsi.InstallationLocation + "\r\n" +
                             "File Name    : " + taskMsi.FileName             + "\r\n" +
                             "Result       : " + ( taskMsi.TaskSucceeded == true ? "Succeeded" : "Failed" ) );
            }
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Permissions;
using System.Xml;
using PrestoCore.BusinessLogic.BusinessEntities;
using PrestoCore.DataAccess;

namespace PrestoCore.BusinessLogic.BusinessComponents
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskXmlModifyLogic : TaskBaseLogic
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskXmlModify"></param>
        public static void Save( TaskXmlModify taskXmlModify )
        {
            ConvertNullPropertiesToEmptyStrings( taskXmlModify );
            Data.SaveObject<TaskXmlModify>( taskXmlModify );
        }

        private static void ConvertNullPropertiesToEmptyStrings( TaskXmlModify taskXmlModify )
        {
            // Two of the properties can be null, but the DB doesn't allow nulls, so make them an empty string.

            if( taskXmlModify.AttributeKey == null ) { taskXmlModify.AttributeKey = string.Empty; }

            if( taskXmlModify.AttributeKeyValue == null ) { taskXmlModify.AttributeKeyValue = string.Empty; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate" )]
        public static ReadOnlyCollection<TaskXmlModify> GetAll()
        {
            return Data.GetObjectList<TaskXmlModify>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TaskXmlModify GetObjectById( int id )
        {
            return Data.GetObjectById<TaskXmlModify>( id );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        [SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes" ), SecurityPermission( SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode )]
        public override void Execute( TaskBase task )
        {
            TaskXmlModify taskXmlModifyOriginal = task as TaskXmlModify;
            TaskXmlModify taskXmlModify = GetTaskXmlModifyWithCustomVariablesResolved(taskXmlModifyOriginal);

            // Store this error to use when throwing exceptions.                
            string taskDetails = string.Format( CultureInfo.CurrentCulture,
                                                "Task ID                  : {0}\r\n" +
                                                "Task Description         : {1}\r\n" +
                                                "XML File                 : {2}\r\n" +
                                                "Node to Change           : {3}\r\n" +
                                                "Attribute Key            : {4}\r\n" +
                                                "Attribute Key Value      : {5}\r\n" +
                                                "Attribute to Change      : {6}\r\n" +
                                                "Attribute to Change Value: {7}\r\n",
                                                taskXmlModify.TaskItemId,
                                                taskXmlModify.Description,
                                                taskXmlModify.XmlPathAndFileName,
                                                taskXmlModify.NodeToChange,
                                                taskXmlModify.AttributeKey,
                                                taskXmlModify.AttributeKeyValue,
                                                taskXmlModify.AttributeToChange,
                                                taskXmlModify.AttributeToChangeValue );

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(taskXmlModify.XmlPathAndFileName);
                XmlElement rootElement = xmlDocument.DocumentElement;

                XmlNodeList xmlNodes;

                // Get the node that the user wants to modify
                if( string.IsNullOrEmpty( taskXmlModify.AttributeKey.Trim() ) )
                {
                    // No attributes necessary to differentiate this node from any others. Get the matching nodes.
                    xmlNodes = rootElement.SelectNodes( taskXmlModify.NodeToChange );
                }
                else
                {
                    // Get the nodes with the specified attributes
                    xmlNodes = rootElement.SelectNodes( string.Format( CultureInfo.InvariantCulture,
                                                                       "descendant::{0}[@{1}='{2}']",
                                                                       taskXmlModify.NodeToChange,
                                                                       taskXmlModify.AttributeKey,
                                                                       taskXmlModify.AttributeKeyValue ) );
                }

                if( xmlNodes == null )
                {
                    // If this is happening, see the comments section below for a possible reason:
                    // -- xmlnode not found because of namespace attribute --
                    throw new Exception( "xmlNode not found.\r\n" /* + taskDetails */ );
                }

                // Make the change
                foreach( XmlNode xmlNode in xmlNodes )
                {
                    if( string.IsNullOrEmpty( taskXmlModify.AttributeToChange ) )
                    {
                        // We're not changing an attribute of the node; we're changing the value (InnerText) of the node itself.
                        xmlNode.InnerText = taskXmlModify.AttributeToChangeValue;
                    }
                    else
                    {
                        if( xmlNode.Attributes[ taskXmlModify.AttributeToChange ] == null )
                        {
                            throw new Exception( "Can't update Attribute to Change because it does not exist in the file." );
                        }

                        // The node has an attribute, so change the attribute's value.
                        xmlNode.Attributes[ taskXmlModify.AttributeToChange ].Value = taskXmlModify.AttributeToChangeValue;
                    }
                }

                xmlDocument.Save(taskXmlModify.XmlPathAndFileName);
                xmlDocument = null;

                taskXmlModifyOriginal.TaskSucceeded = true;

                Utility.Log( taskDetails );
            }
            catch( Exception ex )
            {
                taskXmlModifyOriginal.TaskSucceeded = false;
                Utility.ProcessException( ex.Message + "\r\n" + taskDetails, ex, true );
            }
            finally
            {
                Utility.Log( taskDetails );
            }
        }

        private TaskXmlModify GetTaskXmlModifyWithCustomVariablesResolved(TaskXmlModify taskXmlModifyOriginal)
        {
            TaskXmlModify taskXmlModifyResolved = new TaskXmlModify();

            int groupId = taskXmlModifyOriginal.TaskGroupId;

            taskXmlModifyResolved.AttributeKey           = Utility.ReplaceVariablesWithValues(taskXmlModifyOriginal.AttributeKey, groupId);
            taskXmlModifyResolved.AttributeKeyValue      = Utility.ReplaceVariablesWithValues(taskXmlModifyOriginal.AttributeKeyValue, groupId);
            taskXmlModifyResolved.AttributeToChange      = Utility.ReplaceVariablesWithValues(taskXmlModifyOriginal.AttributeToChange, groupId);
            taskXmlModifyResolved.AttributeToChangeValue = Utility.ReplaceVariablesWithValues(taskXmlModifyOriginal.AttributeToChangeValue, groupId);
            taskXmlModifyResolved.NodeToChange           = Utility.ReplaceVariablesWithValues(taskXmlModifyOriginal.NodeToChange, groupId);
            taskXmlModifyResolved.XmlPathAndFileName     = Utility.ReplaceVariablesWithValues(taskXmlModifyOriginal.XmlPathAndFileName, groupId);

            return taskXmlModifyResolved;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskBase"></param>
        public override void SaveTaskBaseAsConcreteTask( TaskBase taskBase )
        {
            Data.SaveObject<TaskXmlModify>( taskBase as TaskXmlModify );
        }
    }
}

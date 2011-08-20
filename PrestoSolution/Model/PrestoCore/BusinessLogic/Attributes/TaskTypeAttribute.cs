using System;

namespace PrestoCore.BusinessLogic.Attributes
{
    /// <summary>
    /// Attribute for concrete tasks (TaskMsi, etc...) to declare their task type. Classes with these
    /// attributes will get an entry in the DB, using the description as the task type.
    /// </summary>
    internal sealed class TaskTypeAttribute : Attribute
    {
        internal string TaskTypeDescription;
        internal int    TaskTypeId;

        internal TaskTypeAttribute( string taskTypeDescription, int taskTypeId )
        {
            this.TaskTypeDescription = taskTypeDescription;
            this.TaskTypeId          = taskTypeId;
        }
    }
}

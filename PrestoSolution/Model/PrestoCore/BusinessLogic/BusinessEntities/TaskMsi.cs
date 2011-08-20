using System;
using PrestoCore.BusinessLogic.Attributes;

namespace PrestoCore.BusinessLogic.BusinessEntities
{
    /// <summary>
    /// 
    /// </summary>
    [TableName( "TaskMsi" )]
    [Serializable]
    public class TaskMsi : TaskBase
    {
        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "Path" )]
        public string Path { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "FileName" )]
        public string FileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "PassiveInstall" )]
        public byte PassiveInstall { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "ProductGuid" )]
        public string ProductGuid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "WebSite" ), ColumnName( "IisWebSite" )]
        public string IisWebSite { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "Install" )]
        public byte Install { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ColumnName( "InstallationLocation" )]
        public string InstallationLocation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TaskMsi()
        {
            // Some properties can be ignored, depending on whether it's an install or uninstall. So set those to empty strings.
            this.Path                 = string.Empty;
            this.FileName             = string.Empty;
            this.ProductGuid          = string.Empty;
            this.IisWebSite           = string.Empty;
            this.InstallationLocation = string.Empty;
        }
    }
}

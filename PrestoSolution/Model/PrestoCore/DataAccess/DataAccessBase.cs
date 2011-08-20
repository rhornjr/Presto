using System.IO;

namespace PrestoCore.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DataAccessBase
    {
        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Dataset" )]
        protected static PrestoDataset prestoDataset;

        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible" )]
        protected static string dataSetPathAndFileName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathAndFileName"></param>
        public static void InitializeDataSet( string pathAndFileName )
        {
            /**************************************************************************************
             *                             Setup the Dataset                                      *
             **************************************************************************************/
            prestoDataset          = new PrestoDataset();
            dataSetPathAndFileName = pathAndFileName;

            if( File.Exists( dataSetPathAndFileName ) )
            {
                prestoDataset.ReadXml( dataSetPathAndFileName );
                // Need to call AcceptChanges() because it's possible for some other method to call RejectChanges()
                // and that would wipe out the entire dataset if we didn't call AcceptChanges() now.
                prestoDataset.AcceptChanges();
            }
        }
    }
}

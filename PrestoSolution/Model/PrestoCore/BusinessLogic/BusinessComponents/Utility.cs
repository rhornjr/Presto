using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using PrestoCore.BusinessLogic.BusinessEntities;
using PrestoCore.DataAccess;

namespace PrestoCore.BusinessLogic.BusinessComponents
{
    // enum to store keys that will be added to Exception.Data property
    enum ExceptionDataKey
    {
        AlreadyProcessed
    }

    /// <summary>
    /// 
    /// </summary>
    public static class Utility
    {
        #region Public Methods 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathAndFileName"></param>
        public static void SetDataSetPathAndFileName( string pathAndFileName )
        {
            Data.InitializeDataSet( pathAndFileName );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Log( string message )
        {
            try
            {
                string logPath  = ConfigurationManager.AppSettings["PrestoLogPath"].TrimEnd( new char[1] { '\\' } );
                string fileName = "Presto_" + DateTime.Now.ToString( "yyyyMMdd", CultureInfo.InvariantCulture ) + ".log";

                // Create log folder if it doesn't exist.
                if( !Directory.Exists( logPath ) )
                {
                    Directory.CreateDirectory( logPath );
                }

                StreamWriter streamWriter = new StreamWriter( logPath + "\\" + fileName, true );
                streamWriter.Write( "--------------------------------------------\r\n" +
                                    DateTime.Now.ToString() + "\r\n" +
                                    message + "\r\n" );
                streamWriter.Close();
            }
            catch( Exception ex )
            {
                // We don't write to the log here because something is wrong with logging.
                Utility.ProcessException( "Error when trying to write to the log.", ex, false );
                throw;
            }
        }

        /// <summary>
        /// Receive a string and replace the custom variables in it with the values from app.config
        /// </summary>
        /// <param name="stringIn">The text to be modified.</param>
        /// <returns>A new string with the variables replaced with their values.</returns>
        private static string ReplaceVariablesWithValues( string stringIn )
        {
            return ReplaceVariablesWithValues( stringIn, -1 );  // HACK: -1 will not match any groups
        }

        internal static string ReplaceVariablesWithValues( string stringIn, int taskGroupId )
        {
            StringBuilder stringNew = new StringBuilder(stringIn);

            // Get the custom variables and their values from app.config
            NameValueCollection customVariables = new NameValueCollection();
            
            customVariables = ConfigurationManager.GetSection( "customVariables" ) as NameValueCollection;            

            Dictionary<string, string> customVariablesConfigPlusDb = new Dictionary<string, string>();

            // Move the values in the NameValueCollection to the dictionary. (The NameValueCollection is read-only.)
            foreach( string key in customVariables.Keys )
            {
                customVariablesConfigPlusDb.Add( key, customVariables[ key ] );
            }

            // Now get the custom variables from the DB, for this group.
            ReadOnlyCollection<CustomVariable> customVariablesByGroup = CustomVariableLogic.GetCustomVariablesByGroupId( taskGroupId );

            // Custom variables look like this: $(variableKey). So let's add the prefix and suffix to each key.
            string prefix = "$(";
            string suffix = ")";

            // Add these new custom variables to the list.
            foreach( CustomVariable customVariable in customVariablesByGroup )
            {
                customVariablesConfigPlusDb.Add( prefix + customVariable.VariableKey + suffix, customVariable.VariableValue );
            }

            Dictionary<string, string> allCustomVariablesFinal = new Dictionary<string, string>();

            // Custom variable values can themselves contain other custom variables. Resolve those custom variables
            // so that all we are left with is the actual value, with no more pointers to other customer variables.
            // For example, if the value is "C:\Temp\$(tempSubfolder)\$(tempAnotherSubfolder)", resolve the two
            // custom variables so we're left with: "C:\Temp\Snuh\Snuh2".
            foreach (KeyValuePair<string, string> kvp in customVariablesConfigPlusDb)
            {                
                allCustomVariablesFinal.Add(kvp.Key, ResolveAllCustomVariables(kvp.Value, customVariablesConfigPlusDb));
            }

            VerifyCustomVariablesExist(stringIn, allCustomVariablesFinal);

            foreach (string key in allCustomVariablesFinal.Keys)
            {
                stringNew.Replace(key, allCustomVariablesFinal[key]);
            }

            return stringNew.ToString();
        }

        private static string ResolveAllCustomVariables(string incomingString, Dictionary<string, string> customVariablesConfigPlusDb)
        {
            StringBuilder stringNew = new StringBuilder(incomingString);

            foreach (string key in customVariablesConfigPlusDb.Keys)
            {
                stringNew.Replace(key, customVariablesConfigPlusDb[key]);
            }

            return stringNew.ToString();
        }

        private static void VerifyCustomVariablesExist( string sourceString, Dictionary<string, string> customVariables )
        {
            // Make sure the custom variable exists. For example, if sourceString contains a custom variable, that custom variable needs
            // to exist in the customVariables list. If it doesn't, throw an exception.
            
            // Use a regex to find all custom variables in sourceString. The pattern is $(variableName).

            // Explanation of regular expression below:
            // \$  : finds the dollar sign (has to be escaped with the slash)
            // \(  : finds the left paraethesis
            // .+? : . matches any character, + means one or more, ? means ungreedy (will consume characters until the FIRST occurrence of the right parenthesis)
            // \)  : finds the right parenthesis
            Regex regex = new Regex( @"\$\(.+?\)" );

            MatchCollection matchCollection = regex.Matches( sourceString );

            // For each custom variable that we find in the string, make sure it exists in the customVariables list.
            foreach( Match match in matchCollection )
            {
                if( !customVariables.ContainsKey( match.Value ) )
                {
                    throw new MissingMemberException( string.Format( "The task contains a custom variable that does not exist: {0}", match.Value ) );
                }
            }
        }

        /// <summary>
        /// Receive any exception and perform a common set of actions, such as logging.
        /// If the exception passed into this method is a Presto exception,
        /// then we'll need to check and see if it has already been processed. If it
        /// has, then we'll do nothing with it.
        /// The whole point of processing exceptions this way is so that the rest of
        /// the application can process exceptions the same way (by calling this
        /// method). Instead of having to catch Deployment Manager exceptions, and also
        /// catching system exceptions, a developer can just catch Exception and then
        /// call this method to process it. Of course, a developer can still catch
        /// all the different types of exceptions they want to, but now it's their
        /// choice.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <param name="writeToLog"></param>
        public static void ProcessException( string message, Exception ex, bool writeToLog )
        {
            // See if we've already processed this exception.
            // Note: It's basically irrelevant what the value of AlreadyProcessed is. Just the fact that
            //       the key exists means we've already processed it before. (This should be the only
            //       place that we ever add this key/value pair.)
            if( ex.Data.Contains( ExceptionDataKey.AlreadyProcessed ) )
            {
                // We've already processed this exception, so just return
                return;
            }

            // We have not processed this exception. Add the new key/value pair to mark
            // it as processed and continue to the rest of the method.
            ex.Data[ExceptionDataKey.AlreadyProcessed] = true;

            /**************************************************************************************************
             *                  Now put any business logic processing of exceptions here.                     *
             **************************************************************************************************/
            
            // Write the exception info and stack trace to the log file.
            if( writeToLog )
            {
                Utility.Log( message + "\r\n\r\n" + ex.Message + "\r\n\r\n" + System.Environment.StackTrace );
            }                        
        }

        #endregion
    }
}

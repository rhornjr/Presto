using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using PrestoCore.BusinessLogic.Attributes;
using PrestoCore.BusinessLogic.BusinessEntities;

namespace PrestoCore.DataAccess
{
    internal class TypeMetadata
    {
        internal Type   TheType           { get; set; }
        internal string BaseTypeTableName { get; set; }
        internal string DerivedTableName  { get; set; }
    }

    /// <summary>
    /// Information about each property in a class.
    /// </summary>
    internal class PropertyMetadata
    {
        internal string DatabaseColumn { get; set; }
        internal bool   IsPrimaryKey   { get; set; }
        //internal Type   PropertyType   { get; set; }
        internal Object PropertyValue  { get; set; }
    }

    internal class Data : DataAccessBase
    {
        private static Dictionary<string, PropertyMetadata> propertyMetadataMapping = new Dictionary<string, PropertyMetadata>();
        
        private static TypeMetadata  typeMetadata;

        private Data() { }

        internal static void DeleteTaskById( int taskItemId )
        {
            // Get the record by its TaskItemID
            DataRow[] dataRows = prestoDataset.TaskItem.Select( "TaskItemID = " + taskItemId.ToString( CultureInfo.InvariantCulture ) );

            // Delete the row. Because we have our relationships defined in the dataset, and we have elected to use
            // cascading deletes, deleting the TaskItem record will also delete the concrete task (CopyFile, MSI, etc...).
            foreach( DataRow dataRow in dataRows )
            {
                dataRow.Delete();
            }

            SaveDataset();
        }

        internal static void DeleteObject<T>( T entity ) where T : class
        {
            MapPropertyMetadata<T>( entity );

            string tableName = GetTableName( typeof( T ) );

            string primaryKeyField = GetPrimaryKeyField<T>();

            int primaryKeyValue = (int)propertyMetadataMapping[ primaryKeyField ].PropertyValue;

            DataRow dataRow = GetDataRowForTable<T>( tableName, primaryKeyValue );

            dataRow.Delete();

            SaveDataset();
        }

        internal static void SaveObject<T>( T entity ) where T : class
        {
            // If the base type has a table name, then we need to save that first.
            Type   baseType          = typeof( T ).BaseType;
            string baseTypeTableName = GetTableName( baseType );

            string derivedTableName = GetTableName( typeof( T ) );

            MapPropertyMetadata<T>( entity );  // This maps the base and derived classes together.

            int? primaryKeyValue = null;

            if( !string.IsNullOrEmpty( baseTypeTableName ) )
            {
                primaryKeyValue = SaveObjectToTable( baseTypeTableName, true, 0 );
            }

            if( primaryKeyValue == null )
            {
                string primaryKeyColumnName = GetPrimaryKeyField();
                if( propertyMetadataMapping[ primaryKeyColumnName ].PropertyValue != null )
                {
                    primaryKeyValue = (int)propertyMetadataMapping[ primaryKeyColumnName ].PropertyValue;
                }
            }

            SaveObjectToTable( derivedTableName, false, primaryKeyValue );

            SaveDataset();
        }

        internal static ReadOnlyCollection<T> GetObjectList<T>() where T : class
        {
            string tableName = GetTableName( typeof( T ) );

            DataRowCollection dataRows = prestoDataset.Tables[ tableName ].Rows;

            List<T> objects = new List<T>();

            foreach( DataRow dataRow in dataRows )
            {
                objects.Add( GetObjectById<T>( (int)dataRow[ GetPrimaryKeyField<T>() ] ) );
            }

            return objects.AsReadOnly();
        }

        internal static T GetObjectById<T>( int id )
        {
            // Create the ONE object.
            T theObject = Activator.CreateInstance<T>();

            MapPropertyMetadata<T>( theObject );

            DataRow dataRowBase = GetDataRowForTable<T>( GetTypeMetaData<T>().BaseTypeTableName, id );           

            DataRow dataRowDerived = GetDataRowForTable<T>( GetTypeMetaData<T>().DerivedTableName, id );

            if( dataRowBase == null && dataRowDerived == null ) { return default(T); }

            if( dataRowBase != null )
            {
                PopulateObject<T>( dataRowBase, theObject );
            }
            
            PopulateObject<T>( dataRowDerived, theObject );  // Derived table

            return theObject;
        }

        // This is the non-generic version of GetObjectById() above. If we ever want to complete this,
        // all four generic method calls, below, need to have new methods that take the type as a
        // parameter.
        //internal static TaskBase GetTaskBaseById( int id, string fullyQualifiedTypeName )
        //{
        //    // Create the ONE object.
        //    TaskBase taskBase = Activator.CreateInstance( Type.GetType( fullyQualifiedTypeName, true, true ) ) as TaskBase;

        //    MapPropertyMetadata<TaskBase>( taskBase );

        //    DataRow dataRowBase = GetDataRowForTable<TaskBase>( GetTypeMetaData<TaskBase>().BaseTypeTableName, id );

        //    DataRow dataRowDerived = GetDataRowForTable<TaskBase>( GetTypeMetaData<TaskBase>().DerivedTableName, id );

        //    if( dataRowBase == null && dataRowDerived == null ) { return default( TaskBase ); }

        //    if( dataRowBase != null )
        //    {
        //        PopulateObject<TaskBase>( dataRowBase, taskBase );
        //    }

        //    PopulateObject<TaskBase>( dataRowDerived, taskBase );  // Derived table

        //    return taskBase;
        //}

        private static TypeMetadata GetTypeMetaData<T>()
        {
            // If the requested type is the same as the type already stored, return the existing typeMetadata.
            if( typeMetadata != null && typeof( T ) == typeMetadata.TheType ) { return typeMetadata; }

            Type baseType = typeof( T ).BaseType;

            typeMetadata = new TypeMetadata()
            {
                TheType           = typeof( T ),
                BaseTypeTableName = GetTableName( baseType ),
                DerivedTableName  = GetTableName( typeof( T ) )
            };

            return typeMetadata;
        }

        //private static Dictionary<string, PropertyMetadata> GetPropertyMetadataMapping()
        //{            
        //    propertyMetadataMapping = new Dictionary<string, PropertyMetadata>();

        //    //MapPropertyMetadata<

        //    return propertyMetadataMapping;
        //}

        private static DataRow GetDataRowForTable<T>( string tableName, int id )
        {
            if( string.IsNullOrEmpty( tableName ) )
            {
                return null;                
            }

            string primaryKeyField = GetPrimaryKeyField();

            return prestoDataset.Tables[ tableName ].Select( 
                        primaryKeyField + " = " + id.ToString( CultureInfo.InvariantCulture ) ).FirstOrDefault();
        }

        private static void PopulateObject<T>( DataRow dataRow, T theObject )
        {
            // Get all of the public properties of the type.
            List<PropertyInfo> propertyInfoList = new List<PropertyInfo>( typeof(T).GetProperties() );
           
            foreach( PropertyInfo propertyInfo in propertyInfoList )
            {
                // Note: It's possible that the data row won't contain the column. This is because this method can
                //       be populating a base (or child) object separately. If that happens, there will be more
                //       properties in the object than in the data row. (The object contains all of the properties
                //       of the base and child; the data row only contains one of them.)
                if( propertyMetadataMapping[ propertyInfo.Name ].DatabaseColumn != null &&
                    dataRow.Table.Columns.Contains( propertyMetadataMapping[ propertyInfo.Name ].DatabaseColumn ) )
                {
                    propertyInfo.SetValue( theObject, dataRow[ propertyMetadataMapping[ propertyInfo.Name ].DatabaseColumn ], null );
                }
            }
        }

        private static string GetPrimaryKeyField()
        {
            return ( from item in propertyMetadataMapping
                     where item.Value.IsPrimaryKey == true
                     select item.Key ).FirstOrDefault();
        }

        private static int? SaveObjectToTable( string tableName, bool isBaseTable, int? basePrimaryKeyValue )
        {
            string primaryKeyField = GetPrimaryKeyField();

            int? primaryKeyValue = (int?)propertyMetadataMapping[ primaryKeyField ].PropertyValue;

            DataRow[] dataRows = new DataRow[ 1 ];

            if( primaryKeyValue != null )
            {
                string filter = primaryKeyField + " = " + primaryKeyValue.ToString();

                dataRows = new DataRow[ 1 ] { prestoDataset.Tables[ tableName ].Select( filter ).FirstOrDefault() };
            }

            bool isNewRow = false;

            // If record doesn't exist already, create new row.
            if( dataRows[ 0 ] == null )
            {
                isNewRow = true;
                dataRows[ 0 ] = prestoDataset.Tables[ tableName ].NewRow();
            }

            DataRow dataRow = dataRows[ 0 ];

            dataRow.BeginEdit();

            foreach( string propertyName in propertyMetadataMapping.Keys )
            {
                try
                {
                    if( propertyName == primaryKeyField && isBaseTable )
                    {
                        primaryKeyValue = (int)dataRow[ propertyMetadataMapping[ propertyName ].DatabaseColumn ]; 
                        continue;
                    }

                    if( propertyName == primaryKeyField && isBaseTable == false )
                    {
                        primaryKeyValue = basePrimaryKeyValue;
                        if( primaryKeyValue != null )
                        {
                            dataRow[ propertyMetadataMapping[ propertyName ].DatabaseColumn ] = primaryKeyValue;
                        }
                        continue;
                    }

                    dataRow[ propertyMetadataMapping[ propertyName ].DatabaseColumn ] =
                             propertyMetadataMapping[ propertyName ].PropertyValue;
                }
                catch( ArgumentException )
                {
                    // Eat it. The column name doesn't exist in this table (trying to save the base
                    // class to the derived table, or vice-versa.
                    continue;
                }
            }

            if( isNewRow ) { prestoDataset.Tables[ tableName ].Rows.Add( dataRow ); }

            dataRow.EndEdit();

            return primaryKeyValue;
        }

        private static void MapPropertyMetadata<T>( T entity )
        {
            propertyMetadataMapping.Clear();

            // Get all of the public properties of the type.
            List<PropertyInfo> propertyInfoList = new List<PropertyInfo>( typeof(T).GetProperties() );

            foreach( PropertyInfo propertyInfo in propertyInfoList )
            {
                PropertyMetadata propertyMetadata = new PropertyMetadata();

                UpdatePropertyDbColumnnAndPrimaryKey( propertyInfo, propertyMetadata );
                //propertyMetadata.PropertyType   = propertyInfo.PropertyType;
                propertyMetadata.PropertyValue  = propertyInfo.GetValue( entity, null );

                propertyMetadataMapping.Add( propertyInfo.Name, propertyMetadata );
            }
        }

        /// <summary>
        /// Receive a PropertyMetadata and update the Property's DB Column and IsPrimaryKey
        /// </summary>
        private static void UpdatePropertyDbColumnnAndPrimaryKey( PropertyInfo propertyInfo, PropertyMetadata propertyMetadata )
        {
            Attribute[] attributes = Attribute.GetCustomAttributes( propertyInfo );

            foreach( ColumnNameAttribute attribute in attributes.Where( attr => attr is ColumnNameAttribute ) )
            {
                propertyMetadata.DatabaseColumn = attribute.ColumnName;
                propertyMetadata.IsPrimaryKey   = attribute.IsPrimaryKey;
            }
        }

        private static string GetTableName( Type theType )
        {
            Attribute[] attributes = Attribute.GetCustomAttributes( theType );

            foreach( TableNameAttribute attribute in attributes.Where( attr => attr is TableNameAttribute ) )
            {
                return ( (TableNameAttribute)attribute ).TableName;
            }

            return null;
        }

        private static string GetPrimaryKeyField<T>()
        {
            List<PropertyInfo> propertyInfoList = new List<PropertyInfo>( typeof(T).GetProperties() );

            foreach( PropertyInfo propertyInfo in propertyInfoList )
            {
                Attribute[] attributes = Attribute.GetCustomAttributes( propertyInfo );

                foreach( ColumnNameAttribute attribute in attributes.Where( attr => attr is ColumnNameAttribute ) )
                {
                    if( attribute.IsPrimaryKey ) { return attribute.ColumnName; }
                }
            }

            return null;
        }

        private static void SaveDataset()
        {
            prestoDataset.AcceptChanges();
            prestoDataset.WriteXml( dataSetPathAndFileName );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ABC_Supermarket.Stock_Handler
{
    public class StockManager
    {
        //Connection setup
        private CloudTable table;

        public StockManager(string _CloudTableName)
        {
            if (string.IsNullOrEmpty(_CloudTableName))
            {
                throw new ArgumentNullException("Table", "Table name cannot be empty");
            }
            try
            {
                string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=mystorageaccountjm;AccountKey=yzuaqYQhIMWGdtt7zBtFMz7MANLIFEtcHF/DPso+/alvzyDtE3cVki4Yig/Cn9Bg1IoQVlzd0hswviULJRnejg==;EndpointSuffix=core.windows.net";
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                table = tableClient.GetTableReference(_CloudTableName);
                table.CreateIfNotExists();
            }
            catch (StorageException StorageExceptionObj)
            {
                throw StorageExceptionObj;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }
        //Retrieve records
        public List<T> RetrieveEntity<T>(String Query = null) where T : TableEntity, new()
        {
            try
            {
                TableQuery<T> DataTableQuery = new TableQuery<T>();
                if (string.IsNullOrEmpty(Query))
                {
                    DataTableQuery = new TableQuery<T>().Where(Query);
                }
                IEnumerable<T> IDataList = table.ExecuteQuery(DataTableQuery);
                List<T> Datalist = new List<T>();
                foreach (var singleData in IDataList)
                    Datalist.Add(singleData);
                return Datalist;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }
        //Insert and update methods
        public void InsertEntity<T>(T entity, bool forInsert = true) where T : TableEntity, new()
        {
            try
            {
                if (forInsert)
                {
                    var InsertOperation = TableOperation.Insert(entity);
                    table.Execute(InsertOperation);
                }
                else
                {
                    var InsertOrReplaceOperation = TableOperation.InsertOrReplace(entity);
                    table.Execute(InsertOrReplaceOperation);
                }
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }
        //Delete method
        public bool DeleteEntity<T>(T entity) where T : TableEntity, new()
        {
            try
            {
                var DeleteOperation = TableOperation.Delete(entity);
                table.Execute(DeleteOperation);
                return true;
            }
            catch ( Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ABC_Supermarket.BlobHandler
{
    public class BlobManager
    {
        private CloudBlobContainer blobContainer;


        public BlobManager(string ContainerName)
        {
            //Check if container is nullor empty

            if (!string.IsNullOrEmpty(ContainerName)){
                throw new ArgumentNullException("Container", "Container can't be empty");
            }
            try
            {
                string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=mystorageaccountjm;AccountKey=yzuaqYQhIMWGdtt7zBtFMz7MANLIFEtcHF/DPso+/alvzyDtE3cVki4Yig/Cn9Bg1IoQVlzd0hswviULJRnejg==;EndpointSuffix=core.windows.net";
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                blobContainer = cloudBlobClient.GetContainerReference(ContainerName);


                if (blobContainer.CreateIfNotExists())
                {
                    blobContainer.SetPermissions(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Container
                        }
                        );
                }

            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }
        public string UploadFile(HttpPostedFileBase FileToUpload)
        {
            string AbsoluteUri;
            //Check if the posted file is null or not
            if (FileToUpload == null || FileToUpload.ContentLength == 0)
                return null;
            try
            {
                string FileName = Path.GetFileName(FileToUpload.FileName);

                //create the block blob
                CloudBlockBlob blockBlob;
                blockBlob = blobContainer.GetBlockBlobReference(FileName);

                //set object type
                blockBlob.Properties.ContentType = FileToUpload.ContentType;

                //upload Blob
                blockBlob.UploadFromStream(FileToUpload.InputStream);

                //get the Uri or File path
                AbsoluteUri = blockBlob.Uri.AbsoluteUri;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
            return AbsoluteUri;
        }
        //Delete Blob
        public bool DeleteBlob(string AbsoluteUri)
        {
            try
            {
                Uri uriObj = new Uri(AbsoluteUri);
                string BlobName = Path.GetFileName(uriObj.LocalPath);

                //get blob reference
                CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(BlobName);
                //delete blob from container
                blockBlob.Delete();
                return true;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }
        //Retrieve Blobs
        public List<string> BlobList()
        {
            List<string> _blobList = new List<string>();
            foreach (IListBlobItem item in blobContainer.ListBlobs())
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob _blobpage = (CloudBlockBlob)item;
                    _blobList.Add (_blobpage.Uri.AbsoluteUri.ToString());
                }
            }
            return _blobList;
        }
    }
}
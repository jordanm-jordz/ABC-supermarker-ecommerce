using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ABC_Supermarket.Models;
using ABC_Supermarket.Stock_Handler;
using ABC_Supermarket.BlobHandler;

namespace ABC_Supermarket.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(string id)
        {
            //View Editer
            if (!string.IsNullOrEmpty(id))
            {
                //set product name
                StockManager StockManagerObj = new StockManager("Products");

                //Retrieve Product based on their row key
                List<Products> ProductsListObj = StockManagerObj.RetrieveEntity<Products>("RowKey eq'" + id + "'");
                Products ProductObj = ProductsListObj.FirstOrDefault();
                return View(ProductObj);
            }
           
            return View(new Products());
        }
        //Insert and Update product 
        [HttpPost]
        public ActionResult Index(string id,HttpPostedFileBase uploadFile, FormCollection formData)
        {
            Products ProductsObj = new Products();
            ProductsObj.ProductName = formData["ProductName"] == "" ? null : formData["ProductName"];
            ProductsObj.Description = formData["Description"] == "" ? null : formData["Description"];
            double Price;
            if (double.TryParse(formData["Price"], out Price))
            {
               ProductsObj.Price = double.Parse(formData["Price"] == "" ? null : formData["Price"]);
            }
            else
            {
                return View(new Products());
            }

            foreach (string file in Request.Files)
            {
                uploadFile = Request.Files[file];
            }

            //Conmtainer name
            BlobManager BlobManagerObj = new BlobManager("productimages");
            string FileAbstractUri = BlobManagerObj.UploadFile(uploadFile);

            ProductsObj.FilePath = FileAbstractUri.ToString();
            // insert statement
            if (string.IsNullOrEmpty(id))
            {
                ProductsObj.PartitionKey = "Product";
                ProductsObj.RowKey = Guid.NewGuid().ToString();

                StockManager StockMangerObj = new StockManager("Product");
                StockMangerObj.InsertEntity<Products>(ProductsObj, true);
            }
            //Update
            else
            {
                ProductsObj.PartitionKey = "Product";
                ProductsObj.RowKey = id;

                StockManager StockMangerObj = new StockManager("Product");
                StockMangerObj.InsertEntity<Products>(ProductsObj, false);
            }
            return RedirectToAction("Get");
        }
        //Get Product list
        public ActionResult Get()
        {
            StockManager StockManagerObj = new StockManager("Product");
            List<Products> ProductsListObj = StockManagerObj.RetrieveEntity<Products>(null);
            return View(ProductsListObj);
        }
        //Delete
        public ActionResult Delete(string id)
        {
            //retrieve the object to be deleted
            StockManager StockMangerObj = new StockManager("Product");
            List<Products> ProductsListObj = StockMangerObj.RetrieveEntity<Products>("Row eq '" + id + "'");
            Products ProductsObj = ProductsListObj.FirstOrDefault();

            //Delete the Object
            StockMangerObj.DeleteEntity<Products>(ProductsObj);
            return RedirectToAction("Get");

        }

    }
}
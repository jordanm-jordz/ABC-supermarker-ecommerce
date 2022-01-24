using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;
using System.ComponentModel.DataAnnotations;

namespace ABC_Supermarket.Models
{
    public class Products : TableEntity
    {
        public Products() { }

        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string FilePath { get; set; }

    }
}
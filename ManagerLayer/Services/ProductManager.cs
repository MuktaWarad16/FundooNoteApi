using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;

namespace ManagerLayer.Services
{
    public class ProductManager:IProductManager
    {
        public readonly IProductRepository prod;
        public readonly FundooDBContext context;
        public ProductManager(IProductRepository prod, FundooDBContext context)
        {
            this.prod = prod;
            this.context = context;
        }

        public ProductEntity AddProducts(ProductModel model)
        {
            return prod.AddProducts(model);
        }

        public List<ProductEntity> GetProducts(int prodId)
        {
            return prod.GetProducts(prodId);
        }

        public ProductEntity UpdateProducts(int prodId, ProductModel model)
        {
            return prod.UpdateProducts(prodId, model);
        }

        public bool DeleteProduct(int prodId)
        {
            return prod.DeleteProduct(prodId);
        }
    }
}

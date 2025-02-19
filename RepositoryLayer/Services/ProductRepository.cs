using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class ProductRepository:IProductRepository
    {

        private readonly FundooDBContext context;
        private readonly IConfiguration configuration;

        public ProductRepository(FundooDBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public ProductEntity AddProducts(ProductModel model)
        {
            ProductEntity prod = new ProductEntity();
            prod.ProductName = model.ProductName;
            prod.Cost = model.Cost;

            context.Products.Add(prod);
            context.SaveChanges();
            return prod;
        }

        public List<ProductEntity> GetProducts(int prodId)
        {
            var pList = context.Products.Where(p => p.ProductId == prodId);
            if (pList == null)
            {
                return null;
            }
            return pList.ToList();
        }

        public ProductEntity UpdateProducts(int prodId, ProductModel model)
        {
            var product = context.Products.FirstOrDefault(p => p.ProductId == prodId);
            if (product == null)
            {
                return null;
            }
            product.ProductName = model.ProductName;
            product.Cost = model.Cost;
            context.Update(product);
            context.SaveChanges();
            return product;
        }

        public bool DeleteProduct(int prodId)
        {
            var product = context.Products.FirstOrDefault(p => p.ProductId == prodId);
            if (product == null)
            {
                return false;
            }
            context.Products.Remove(product);
            context.SaveChanges();
            return true;
        }

    }
}

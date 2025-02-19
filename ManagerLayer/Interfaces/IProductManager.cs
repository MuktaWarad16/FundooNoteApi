using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace ManagerLayer.Interfaces
{
    public interface IProductManager
    {
        public ProductEntity AddProducts(ProductModel model);

        public List<ProductEntity> GetProducts(int prodId);

        public ProductEntity UpdateProducts(int prodId, ProductModel model);

        public bool DeleteProduct(int prodId);
    }
}

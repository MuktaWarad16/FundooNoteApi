using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface IProductRepository
    {
        public ProductEntity AddProducts(ProductModel model);

        public List<ProductEntity> GetProducts(int prodId);

        public ProductEntity UpdateProducts(int prodId, ProductModel model);

        public bool DeleteProduct(int prodId);
    }
}

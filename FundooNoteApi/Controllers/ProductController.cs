using CommonLayer.Models;
using CommonLayer;
using System.Collections.Generic;
using System;
using ManagerLayer;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System.Threading.Tasks;
using System.Linq;

namespace FundooNoteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly FundooDBContext context;
        private readonly IProductManager manager;

        public ProductController(IProductManager manager, FundooDBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.manager = manager;
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddProducts(ProductModel model)
        {
            try
            {
                if (model == null)
                {
                    return null;
                }
                var product = manager.AddProducts(model);
                if (product != null)
                {
                    return Ok(new ResponseModel<ProductEntity> { success = true, message = "Products added Successfully", Data = product });
                }
                return BadRequest(new ResponseModel<ProductEntity> { success = false, message = "Failed to add Products" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("get")]
        public IActionResult GetProducts(int productId)
        {
            try
            {
                var products = manager.GetProducts(productId);

                if (products != null)
                {
                    return Ok(new ResponseModel<List<ProductEntity>> { success = true, message = "Product retrieved successfully", Data = products });
                }
                return BadRequest(new ResponseModel<List<ProductEntity>> { success = false, message = "Product retrieved successfully" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateProduct(int productId, ProductModel model)
        {
            try
            {
                var product = manager.UpdateProducts(productId, model);
                if (product != null)
                {
                    return Ok(new ResponseModel<ProductEntity> { success = true, message = "Product updated successfully", Data = product });
                }
                return BadRequest(new ResponseModel<List<ProductEntity>> { success = false, message = "failed to update product" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult DeleteProduct(int productId)
        {
            try
            {
                var idDeleted = manager.DeleteProduct(productId);
                if (idDeleted == true)
                {
                    return Ok(new ResponseModel<bool> { success = true, message = "Product deleted successfully", Data = idDeleted });
                }
                return BadRequest(new ResponseModel<bool> { success = false, message = "failed to delete product" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
     }
}

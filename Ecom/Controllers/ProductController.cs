using System.Collections.Generic;
using System.Threading.Tasks;
using Ecom.Controllers.APi;
using Microsoft.AspNetCore.Mvc;
using Models.Request.Product;

namespace Ecom.Controllers
{
    [Route("api/v1/[controller]")]
    public class ProductController : BaseController
    {
        [HttpGet("getAllProducts")]
        public async Task<IActionResult> getAllProducts()
        {
            return Ok(await _ProductService.getAllProducts(false));
        }

        [HttpGet("getProductByCat")]
        public async Task<IActionResult> getProductByCat(long catId)
        {
            return Ok(await _ProductService.getProductByCat(false,catId));
        }

        [HttpGet("getDiscountedProductByCat")]
        public async Task<IActionResult> getDiscountedProductByCat(long catId)
        {
            return Ok(await _ProductService.getProductByCat(true, catId));
        }

        [HttpGet("getDiscountedProducts")]
        public async Task<IActionResult> getDiscountedProducts()
        {
            return Ok(await _ProductService.getAllProducts(true));
        }

        [HttpGet("getSellerProducts")]
        public async Task<IActionResult> getSellerProducts(long sellerId)
        {
            return Ok(await _ProductService.getProductsOfSeller(sellerId));
        }

        [HttpPost("addNewProduct")]
        public async Task<IActionResult> addNewProduct([FromForm]InsertProductRequestModel request)
        {
            return Ok(await _ProductService.addProduct(request));
        }


        [HttpPut("updateProduct")]
        public async Task<IActionResult> updateProduct([FromForm]InsertProductRequestModel request)
        {
            return Ok(await _ProductService.updateProduct(request));
        }

        [HttpGet("deleteProduct")]
        public async Task<IActionResult> deleteProduct(long productId)
        {
            return Ok(await _ProductService.deleteProduct(productId));
        }

        [HttpGet("getCategories")]
        public async Task<IActionResult> getCategories()
        {
            return Ok(await _ProductService.getCategories());
        }

        [HttpPost("insertCategory")]
        public async Task<IActionResult> insertCategory([FromBody]InsertCategory request)
        {
            return Ok(await _ProductService.insertCategory(request));
        }

        [HttpGet("deleteCategory")]
        public async Task<IActionResult> deleteCategory(long id)
        {
            return Ok(await _ProductService.deleteCategory(id));
        }
    }
}

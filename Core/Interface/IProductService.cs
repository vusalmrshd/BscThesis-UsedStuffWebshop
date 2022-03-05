using System;
using System.Threading.Tasks;
using Models.Request.Product;
using Models.Response.CoreResponse;

namespace Core.Interface
{
    public interface IProductService
    {
        Task<CoreResponseModel> addProduct(InsertProductRequestModel request);
        Task<CoreResponseModel> updateProduct(InsertProductRequestModel request);
        Task<CoreResponseModel> deleteProduct(long productId);
        Task<CoreResponseModel> getProductsOfSeller(long sellerId);
        Task<CoreResponseModel> getAllProducts(bool isDiscount);
        Task<CoreResponseModel> getProductByCat(bool isDiscount,long catId);
        Task<CoreResponseModel> insertCategory(InsertCategory request);
        Task<CoreResponseModel> getCategories();
        Task<CoreResponseModel> deleteCategory(long id);

    }
}

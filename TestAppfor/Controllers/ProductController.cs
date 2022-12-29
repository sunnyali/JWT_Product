using DAL.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static ViewModel.Enum.Enums;
using static ViewModel.Product.ProductViewModel;

namespace TestAppfor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {

        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices ?? throw new ArgumentNullException("ProductController_API");
        }

        #region API

        [HttpPost]
        [Authorize]
        [Route("AddNewProduct")]
        [ActionName("AddNewProduct")]
        public IActionResult AddNewProduct(AddProductReq request)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    int User_ID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    var isAdded = _productServices.AddNewProduct(request, User_ID, out string errMsg);
                    response = FormatResponse(errMsg, HttpMethod.Post, isAdded, "New product added successfully");
                }
                else
                {
                    var errors = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    response = FormatResponse(errors, HttpMethod.Post, null);
                }


            }
            catch (Exception ex)
            {
                response = ConstructReturnResponse(104, "Invalid or unable to process your request");
            }

            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        [Route("GetUserProducts")]
        [ActionName("GetUserProducts")]
        public IActionResult GetUserProducts()
        {
            try
            {
                int User_ID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var transationList = _productServices.GetUserProducts(User_ID, out string errMsg);
                response = FormatResponse(errMsg, HttpMethod.Get, transationList);
            }
            catch (Exception ex)
            {
                response = ConstructReturnResponse(400, "Invalid or unable to process your request");
            }

            return Ok(response);
        } 
        #endregion
    }
}

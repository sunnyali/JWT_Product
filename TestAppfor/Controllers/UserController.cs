using DAL.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.User;
using static ViewModel.Enum.Enums;

namespace TestAppfor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {

        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices ?? throw new ArgumentNullException("UserController_API");
        }

        #region API
        [HttpPost]
        [Route("AddNewUser")]
        [ActionName("AddNewUser")]
        public IActionResult AddNewUser(AddUserReq request)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var isAdded = _userServices.AddNewUser(request, out string errMsg);
                    response = FormatResponse(errMsg, HttpMethod.Post, isAdded, "New User added successfully");
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

        [HttpPost]
        [Route("AuthenticateUser")]
        [ActionName("AuthenticateUser")]
        public IActionResult AuthenticateUser(LoginReq request)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var userLoginData = _userServices.AuthenticateUser(request, out string errMsg);
                    response = FormatResponse(errMsg, HttpMethod.Post, userLoginData, "Logged in successfully");

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

                response = ConstructReturnResponse(400, "Unable to login, please try again later");
            }

            return Ok(response);
        } 
        #endregion
    }
}

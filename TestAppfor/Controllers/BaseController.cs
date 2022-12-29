using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Common;
using ViewModel.Enum;
using static ViewModel.Common.CommonViewModel;

namespace TestAppfor.Controllers
{
   
    [ApiController]
    public class BaseController : ControllerBase
    {
        public Result response;
        private static readonly string SessionErrorMsg = "Invalid session or expired";
        public static Result ConstructReturnResponse(int statusCode, string message, object responseData = null)
        {
            return new Result
            {
                StatusCode = statusCode,
                Message = message,
                Data = responseData
            };
        }
        public static Result FormatResponse(string errMsg, Enums.HttpMethod httpMethod, object responseData = null, string successMsg = "")
        {
            var result = new Result();
            if (!string.IsNullOrEmpty(errMsg))
            {
                if (errMsg == SessionErrorMsg)
                {
                    result = ConstructReturnResponse(103, errMsg); // means session expired, must login again
                }
                else
                {
                    result = ConstructReturnResponse(104, errMsg);
                }
            }
            else
            {
                if (responseData != null)
                {
                    switch (httpMethod)
                    {
                        case Enums.HttpMethod.Put:
                            result = ConstructReturnResponse(100, successMsg == "" ? "Updated Successfully" : successMsg, responseData);
                            break;

                        case Enums.HttpMethod.Get:
                        case Enums.HttpMethod.Delete:
                            result = ConstructReturnResponse(100, successMsg == "" ? "Action-Succeeded" : successMsg, responseData);
                            break;

                        case Enums.HttpMethod.Post:
                            result = ConstructReturnResponse(101, successMsg == "" ? "Created Successfully" : successMsg, responseData); //status code 201 
                            break;
                    }
                }
                else
                {
                    result = ConstructReturnResponse(105, "No results found", null);
                }
            }

            return result;
        }
    }
}

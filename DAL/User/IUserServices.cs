using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.User;

namespace DAL.User
{
    public interface IUserServices
    {
        /// <summary>
        /// Authenticate user (login service)
        /// </summary>
        /// <param name="request">request contains username and password</param>
        /// <param name="errMsg">returns exception or error message</param>
        /// <returns>returns user login id (Guid)</returns>
        LoginRes AuthenticateUser(LoginReq request, out string errMsg);
        /// <summary>
        /// Adds new user (Registration)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="errMsg"></param>
        /// <returns>returns true/false - true means it is registered</returns>
        bool AddNewUser(AddUserReq request, out string errMsg);
    }
}

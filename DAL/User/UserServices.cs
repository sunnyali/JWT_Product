using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.User;
using DAL.Helper;
using Model;
using DAL.Providers;

namespace DAL.User
{
    public class UserServices : IUserServices
    {
        public Properties _properties = new();
        public DBContext objCW = new();


        public bool AddNewUser(AddUserReq request, out string errMsg)
        {
            var result = false;
            errMsg = string.Empty;

            try
            {
                string HashPassword = ServiceUtility.GetHash(request.Password.Trim());
                var spResult = "";
                DataTable dataTable = new DataTable();
                DataSet ds = new DataSet();
                using (var con = objCW.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("USP_AddUser", con);
                    cmd.Connection = con;
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name ", request.Name);
                    cmd.Parameters.AddWithValue("@Password", HashPassword);
                    cmd.Parameters.AddWithValue("@Email", request.Email);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    da.Dispose();
                    dataTable = ds.Tables[0];
                    spResult = dataTable.Rows[0]["Result"].ToString();

                }

                string spresult = spResult.ToString();
                var splitResult = ServiceUtility.SplitString(spresult, '|');

                switch (splitResult[0])
                {
                    case "201":
                        result = true; 
                        break;
                    case "403":
                        errMsg = "User with same EmailID already exists";
                        break;
                    default:
                        errMsg = "Invalid request";
                        break;
                }
            }
            catch (Exception ex)
            {
                //Error Log
                
            }

            return result;
        }
        public LoginRes AuthenticateUser(LoginReq request, out string errMsg)
        {
            LoginRes result = new();
            errMsg = string.Empty;
            try
            {

                string HashPassword = ServiceUtility.GetHash(request.Password.Trim());
                string spResult = null;
                DataTable dataTable = new DataTable();
                DataSet ds = new DataSet();
                using (var con = objCW.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("USP_AuthenticateUser", con);
                    cmd.Connection = con;
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", request.Email);
                    cmd.Parameters.AddWithValue("@Password",HashPassword);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    da.Dispose();
                    dataTable = ds.Tables[0];
                    spResult = dataTable.Rows[0]["Result"].ToString();

                }

                if (!string.IsNullOrEmpty(spResult))
                {
                    string spresult = spResult.ToString();
                    var splitResult = ServiceUtility.SplitString(spresult, '|');

                    switch (splitResult[0])
                    {
                        case "200":
                 
                            result.Login_ID = splitResult[1];

                            // get token
                            var tokenSvc = new TokenProvider();
                            var tokenID = Guid.NewGuid();

                            var token = tokenSvc.CreateToken(Convert.ToString(result.Login_ID));
                            if (token != null)
                            {
                                result.Token = token.Access_Token;
                            }
                            else
                            {
                                errMsg = "unable to generate access token";
                            }
                            break;

                        case "401":
                            errMsg = "Invalid username and password";
                            break;

                        case "404":
                        case "400":
                            errMsg = "Invalid username and password";
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                //Error Log
            }
            return result;
        }
    }
}

using DAL.Helper;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Product;

namespace DAL.Product
{
    public class ProductServices : IProductServices
    {
        public Properties _properties = new();
        public DBContext objCW = new();
        public bool AddNewProduct(ProductViewModel.AddProductReq request, int User_ID, out string errMsg)
        {
            var result = false;
            errMsg = string.Empty;

            try
            {
                
                var spResult = "";
                DataTable dataTable = new DataTable();
                DataSet ds = new DataSet();
                using (var con = objCW.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("USP_AddProduct", con);
                    cmd.Connection = con;
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Tittle ", request.Tittle);
                    cmd.Parameters.AddWithValue("@Description", request. Description);
                    cmd.Parameters.AddWithValue("@User_ID", User_ID);

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
                        errMsg = "Product with same name already exists";
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

        public object GetUserProducts(int User_ID, out string errMsg)
        {
            errMsg = string.Empty;
            DataTable dataTable = new DataTable();
            using (var con = objCW.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("USP_GetUserProducts", con);
                cmd.Connection = con;
                cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@User_ID", User_ID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);
                da.Dispose();

            }
            if (dataTable.Rows.Count > 0)
            {
                return dataTable;

            }
            else
            {
                errMsg = "No record found";
            }
            return errMsg;
        }
    }
}

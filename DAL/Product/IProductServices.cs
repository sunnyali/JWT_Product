using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ViewModel.Product.ProductViewModel;

namespace DAL.Product
{
   public interface IProductServices
    {
        bool AddNewProduct(AddProductReq request, int User_ID, out string errMsg);
        Object GetUserProducts(int User_ID, out string errMsg);
    }
}

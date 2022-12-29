using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Common
{
    public class CommonViewModel
    {

        public class Result
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
            public object Data { get; set; }
        }
        public class ClaimIdentityResponse
        {
            public string ID { get; set; }
            public bool IsValidUser { get; set; }
            public string UserName { get; set; }
            public Guid User_ID { get; set; }
            public Guid Login_ID { get; set; }
            public Guid Role_ID { get; set; }
            public string Customer_Email { get; set; }
            public string Role { get; set; }
            public string Message { get; set; }
        }
    }
}

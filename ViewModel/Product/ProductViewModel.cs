using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Product
{
   public class ProductViewModel
    {
        public class AddProductReq
        {
            [Required(ErrorMessage = "{0} is required")]
            [StringLength(50, ErrorMessage = "{0} Max 50 character allowed")]
            public string Tittle { get; set; }

            [Required(ErrorMessage = "{0} is required")]
            public string Description { get; set; }

        }
    }
}

using E_ComerCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace E_ComerCore.Areas.Admin.Models.ViewModels
{
    public class ProductView
    {
        public Product Product { get; set; }

        public List<SelectListItem> Category { get; set; }
    }
}

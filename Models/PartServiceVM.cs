using DALProject.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace PLProj.Models
{
    public class PartServiceVM
    {
        public virtual PartService partService { get; set; }
        [ValidateNever]

        public IEnumerable<SelectListItem>? CategoryList { get; set; }
    }
}

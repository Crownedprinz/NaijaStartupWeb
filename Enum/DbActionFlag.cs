using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaijaStartupWeb.Enum
{
    public enum DbActionFlag
    {
        [Display(Name = "Create")]
        Create = 1,
        [Display(Name = "Update")]
        Update = 2,
        [Display(Name = "Delete")]
        Delete = 3,
    }
}
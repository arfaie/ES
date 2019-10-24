using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcInternetShop.Models.Enums
{
    public enum PaymentTypes
    {
        Cash = 1,
        cardReader=2,
        Check = 3,
        bankJar=4,
        Online=5
    }
}
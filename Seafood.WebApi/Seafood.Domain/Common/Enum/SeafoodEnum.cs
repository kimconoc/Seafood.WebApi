using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seafood.Domain.Common.Enum
{
    public enum ImageTypeEnum
    {
        [Description("Get list Image Product")]
        Product = 0,
        [Description("Get list Image ShopSeefood")]
        Shop = 1,
        [Description("Get list Image More")]
        More = 2,
    }
}

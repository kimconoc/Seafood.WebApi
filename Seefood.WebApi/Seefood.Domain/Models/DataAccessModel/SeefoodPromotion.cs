﻿using Seefood.Domain.Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seefood.Domain.Models.DataAccessModel
{
    public class SeefoodPromotion : VBaseModel
    {
        public string ShopCode { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
    }
}

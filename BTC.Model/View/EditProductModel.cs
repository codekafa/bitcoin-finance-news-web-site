﻿using BTC.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Model.View
{
    public class EditProductModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Uri { get; set; }
        public decimal? Price { get; set; }
        public string Tags { get; set; }
        public bool IsPublish { get; set; }
        public List<ProductPhotos> Photos { get; set; }

    }
}

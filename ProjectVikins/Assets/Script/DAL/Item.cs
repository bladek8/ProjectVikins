using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Assets.Script.DAL
{
    public class Item
    {
        [DisplayName("Key")]
        public int ItemId { get; set; }
        public int? ItemTypeId { get; set; }
        public int? Amount { get; set; }
        public string DescriptionText { get; set; }
        public string Name { get; set; }
        public double? Health { get; set; }
        public double? Strenght { get; set; }
    }
}

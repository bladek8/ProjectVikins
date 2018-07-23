using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.DAL.Shared
{
    public class Item
    {
        public int ItemId { get; set; }
        public int ItemTypeId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public float InitialX { get; set; }
        public float InitialY { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Models
{
    public class HealthItemViewModel
    {
        public int ItemId { get; set; }
        public DAL.ItemTypes ItemTypeId { get; set; }
        public string Name { get; set; }
        public float Health { get; set; }
        public int Amount { get; set; }
        public float InitialX { get; set; }
        public float InitialY { get; set; }
    }
}

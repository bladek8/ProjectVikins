using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Models
{
    public class ItemViewModel
    {
        public int? ItemId { get; set; }
        public int? ItemTypeId { get; set; }
        public int? Amount { get; set; }
        public string DescriptionText { get; set; }
        public string Name { get; set; }
        public GameObject Prefab { get; set; }
        public Sprite Icon { get; set; }
        public double? Health { get; set; }
        public double? Strenght { get; set; }
    }
}

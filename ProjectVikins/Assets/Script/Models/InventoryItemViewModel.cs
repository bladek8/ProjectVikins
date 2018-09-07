using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Models
{
    public class InventoryItemViewModel
    {
        public int? InventoryItemId { get; set; }
        public int? ItemId { get; set; }
        public int? Amount { get; set; }
        public Sprite Icon { get; set; }
        public GameObject Prefab { get; set; }
    }
}

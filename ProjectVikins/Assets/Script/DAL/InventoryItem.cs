using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.DAL
{
    [Serializable]
    public class InventoryItem
    {
        public int InventoryItemId { get; set; }
        public int ItemTypeId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }
        public string DescriptionText { get; set; }
        public GameObject Prefab { get; set; }
        public Sprite Icon { get; set; }
    }
}

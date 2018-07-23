using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.DAL
{
    [Serializable]
    public class InventoryItem
    {
        public int InventoryItemId { get; set; }
        public int ItemTypeId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }
    }
}

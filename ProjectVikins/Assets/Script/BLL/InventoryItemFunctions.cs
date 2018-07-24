using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.BLL.Shared;
using Assets.Script.DAL;
using UnityEngine;

namespace Assets.Script.BLL
{
    public class InventoryItemFunctions : BLLContextFuntions<DAL.InventoryItem>
    {
        public InventoryItemFunctions()
            : base("InventoryItemId")
        {
            SetListContext();
        }

        public override int Create(InventoryItem data)
        {
            if (ListContext.Any(x => x.ItemId == data.ItemId))
                ListContext.Where(x => x.ItemId == data.ItemId).First().Amount += data.Amount;
            else
                ListContext.Add(data);
            return data.InventoryItemId;
        }

        public int Create(object data)
        {
            var inventoryItem = InventoryItemCast(data);
            if (ListContext.Any(x => x.ItemId == inventoryItem.ItemId))
                ListContext.Where(x => x.ItemId == inventoryItem.ItemId).First().Amount += inventoryItem.Amount;
            else
                ListContext.Add(inventoryItem);
            return inventoryItem.InventoryItemId;
        }

        public InventoryItem InventoryItemCast(object data)
        {
            var inventoryItem = new InventoryItem
            {
                ItemId = Convert.ToInt32(data.GetType().GetProperty("ItemId").GetValue(data, null)),
                ItemTypeId = Convert.ToInt32(data.GetType().GetProperty("ItemTypeId").GetValue(data, null)),
                Amount = Convert.ToInt32(data.GetType().GetProperty("Amount").GetValue(data, null)),
                Prefab = (GameObject)data.GetType().GetProperty("Prefab").GetValue(data, null)
            };
            return inventoryItem;
        }

        public override void SetListContext()
        {
            this.ListContext = ProjectVikingsContext.InventoryItens;
        }
    }
}

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
                ListContext.SingleOrDefault(x => x.ItemId == inventoryItem.ItemId).Amount += inventoryItem.Amount;
            else
                ListContext.Add(inventoryItem);
            return inventoryItem.InventoryItemId;
        }

        public InventoryItem InventoryItemCast(object data)
        {
            var inventoryItem = new InventoryItem
            {
                ItemId = Convert.ToInt32(data.GetType().GetProperty("ItemId").GetValue(data, null)),
                Amount = Convert.ToInt32(data.GetType().GetProperty("Amount").GetValue(data, null)),
                InventoryItemId = ListContext.Count + 1
            };
            return inventoryItem;
        }

        public override void SetListContext()
        {
            this.ListContext = ProjectVikingsContext.InventoryItens.Entity;
        }

        public void DecreaseAmount(int? itemId)
        {
            GetData().FirstOrDefault(x => x.ItemId == itemId).Amount--;
        }
    }
}

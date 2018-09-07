using Assets.Script.BLL.Shared;
using Assets.Script.DAL;
using Assets.Script.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Script.Helpers;

namespace Assets.Script.BLL
{
    public class ItemFunctions : BLLFunctions<Item, ItemViewModel>
    {
        private readonly ItemTypeFunctions itemTypeFunctions = new ItemTypeFunctions();

        public ItemFunctions() 
            : base("ItemId")
        {
        }

        public override int Create(ItemViewModel model)
        {
            throw new NotImplementedException();
        }

        public override Item GetDataByViewModel(ItemViewModel model)
        {
            throw new NotImplementedException();
        }

        public override List<Item> GetDataByViewModel(IEnumerable<ItemViewModel> model)
        {
            throw new NotImplementedException();
        }

        public override ItemViewModel GetDataViewModel(Item data)
        {
            return new ItemViewModel {
            Amount = data.Amount,
            ItemId = data.ItemId,
            DescriptionText = data.DescriptionText,
            Health = data.Health,
            ItemTypeId = data.ItemTypeId,
            Name = data.Name,
            Strenght = data.Strenght
            };
        }

        public override List<ItemViewModel> GetDataViewModel(IEnumerable<Item> data)
        {
            return (from y in data
            select new ItemViewModel
            {
                Amount = y.Amount,
                ItemId = y.ItemId,
                DescriptionText = y.DescriptionText,
                Health = y.Health,
                ItemTypeId = y.ItemTypeId,
                Name = y.Name,
                Strenght = y.Strenght
            }).ToList();
        }

        public override void SetListContext()
        {
            this.ListContext = ProjectVikingsContext.Item.Data;
        }

        public override void SetListModel()
        {
            this.ListModel = ProjectVikingsContext.ItemViewModel.Entity;
        }

        public override int SetModel(ItemViewModel model)
        {
            this.ListModel.Add(model);
            return model.ItemId.Value;
        }

        public ItemViewModel GetDataByIcon(Sprite icon)
        {
            return this.ListModel.FirstOrDefault(x => x.Icon == icon);
        }

        public List<ItemAttributtesViewModel> GetItemAtributtes(int itemId)
        {
            var model = GetDataById(itemId);
            var itemAttributtes = new List<ItemAttributtesViewModel>();

            var itemTypes = itemTypeFunctions.GetData();
            foreach (var prop in model.GetType().GetProperties())
            {
                if (!itemTypes.Any(x => x.Name == prop.Name)) continue;
                itemAttributtes.Add(new ItemAttributtesViewModel {
                    Name = prop.Name,
                    Value = Convert.ToDouble(model.GetType().GetProperty(prop.Name).GetValue(model, null))
                });

            }
            return itemAttributtes;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.BLL.Shared;
using Assets.Script.DAL;
using Assets.Script.Models;

namespace Assets.Script.BLL
{
    public class StrenghtItemFunctions : BLLListFunctions<StrenghtItem, StrenghtItemViewModel>
    {
        public StrenghtItemFunctions() 
            : base("StrenghtItemId")
        {
            SetListContext();
        }

        public override StrenghtItemViewModel GetDataViewModel(StrenghtItem data)
        {
            return new StrenghtItemViewModel()
            {
                Amount = data.Amount,
                Strenght = data.Strenght,
                InitialX = data.InitialX,
                InitialY = data.InitialY,
                ItemId = data.ItemId,
                Name = data.Name,
                ItemTypeId = (ItemTypes)data.ItemTypeId
            };
        }

        public override List<StrenghtItemViewModel> GetDataViewModel(IEnumerable<StrenghtItem> data)
        {
            return (from y in data
                    select new StrenghtItemViewModel()
                    {
                        Amount = y.Amount,
                        Strenght = y.Strenght,
                        InitialX = y.InitialX,
                        InitialY = y.InitialY,
                        ItemId = y.ItemId,
                        Name = y.Name,
                        ItemTypeId = (ItemTypes)y.ItemTypeId
                    }).ToList();
        }

        public override void SetListContext()
        {
            this.ListContext = ProjectVikingsContext.StrenghtItens;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.BLL.Shared;
using Assets.Script.DAL;
using Assets.Script.Models;

namespace Assets.Script.BLL
{
    public class HealthItemFunctions : BLLListFunctions<HealthItem, HealthItemViewModel>
    {
        public HealthItemFunctions() 
            : base("HealthItemId")
        {
            SetListContext();
        }

        public override HealthItemViewModel GetDataViewModel(HealthItem data)
        {
            return new HealthItemViewModel()
            {
                Amount = data.Amount,
                Health = data.Health,
                InitialX = data.InitialX,
                InitialY = data.InitialY,
                ItemId = data.ItemId,
                Name = data.Name,
                ItemTypeId = (ItemTypes)data.ItemTypeId
            };
        }

        public override List<HealthItemViewModel> GetDataViewModel(IEnumerable<HealthItem> data)
        {
            return (from y in data
                    select new HealthItemViewModel() {
                        Amount = y.Amount,
                        Health = y.Health,
                        InitialX = y.InitialX,
                        InitialY = y.InitialY,
                        ItemId = y.ItemId,
                        Name = y.Name,
                        ItemTypeId = (ItemTypes)y.ItemTypeId
                    }).ToList();
        }

        public override void SetListContext()
        {
            this.ListContext = ProjectVikingsContext.HealthItens;
        }
    }
}

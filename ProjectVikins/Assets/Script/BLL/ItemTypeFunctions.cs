using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.BLL.Shared;
using Assets.Script.DAL;

namespace Assets.Script.BLL
{
    public class ItemTypeFunctions : BLLListFunctions<DAL.ItemType, DAL.ItemType>
    {
        public ItemTypeFunctions() 
            : base("ItemTypeId")
        {
        }
        
        public override ItemType GetDataViewModel(ItemType data)
        {
            return data;
        }

        public override List<ItemType> GetDataViewModel(IEnumerable<ItemType> data)
        {
            return data.ToList();
        }

        public override void SetListContext()
        {
            this.ListContext = ProjectVikingsContext.ItemTypes.Data;
        }
    }
}

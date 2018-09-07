using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.BLL;
using UnityEngine;

namespace Assets.Script.Controller
{
   public class ItensController
    {
        private readonly ItemFunctions itemFunctions = new ItemFunctions();
        //private readonly StrenghtItemFunctions StrenghtItemFunctions = new StrenghtItemFunctions();

        public Models.ItemViewModel GetInitialData(int id, GameObject prefab, Sprite icon)
        {
            var data = itemFunctions.GetDataById(id);
            if (data == null)
                return null;
            var model = itemFunctions.GetDataViewModel(data);
            model.Prefab = prefab;
            model.Icon = icon;

            itemFunctions.SetModel(model);
            return model;
        }

        //public Models.StrenghtItemViewModel StrenghtGetInitialData(Vector3 position)
        //{
        //    var data = StrenghtItemFunctions.GetDataByInitialPosition(position);
        //    if (data == null)
        //        return null;
        //    var model = StrenghtItemFunctions.GetDataViewModel(data);
        //    return model;
        //}
    }
}

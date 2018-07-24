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
        private readonly HealthItemFunctions HealthItemFunctions = new HealthItemFunctions();
        
        public Models.HealthItemViewModel HealthGetInitialData(Vector3 position)
        {
            var data = HealthItemFunctions.GetDataByInitialPosition(position);
            if (data == null)
                return null;
            var model = HealthItemFunctions.GetDataViewModel(data);
            return model;
        }
    }
}

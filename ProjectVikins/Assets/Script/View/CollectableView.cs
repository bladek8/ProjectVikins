using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.View
{
    public class CollectableView : MonoBehaviour
    {
        DAL.HealthItem data;
        public Controller.ItensController itensController;

        private void Awake()
        {
            itensController = new Controller.ItensController();
            data = itensController.HealthGetInitialData(transform.position);
        }

        public DAL.HealthItem GetItemModel()
        {
            Destroy(gameObject);
            print("Pegou");
            return data;
        }
    }
}

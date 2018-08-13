using Assets.Script.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.View
{
    public class CollectableView : MonoBehaviour
    {
        Models.HealthItemViewModel model;
        public Controller.ItensController itensController;
        private BoxCollider2D BoxCollider2D;
        public GameObject Prefab;
        public Sprite Icon;
        public string DescriptionText;

        private void Awake()
        {
            DescriptionText = "Coco: Aumenta 5 de vida";
            itensController = new Controller.ItensController();
            model = itensController.HealthGetInitialData(transform.position);
            if (model == null)
            {
                Destroy(this.gameObject);
                return;
            }
            model.Prefab = Prefab;
            model.Icon = Icon;
            model.DescriptionText = DescriptionText;
            BoxCollider2D = GetComponent<BoxCollider2D>();
            transform.position = Utils.SetPositionZ(transform, BoxCollider2D.bounds.min.y);
        }

        public Models.HealthItemViewModel GetItemModel()
        {
            if (DAL.ProjectVikingsContext.InventoryItens.Count >= InventoryView.space && DAL.ProjectVikingsContext.InventoryItens[0].ItemId != model.ItemId && DAL.ProjectVikingsContext.InventoryItens[1].ItemId != model.ItemId)
            {
                print("Sem espaço no inventario");
                return null;
            }
            else
            {
                Destroy(gameObject);
                return model;
            }
        }
    }
}

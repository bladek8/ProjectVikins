using Assets.Script.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.View
{
    public class ItemView : MonoBehaviour
    {
        public int id;
        public GameObject Prefab;
        public Sprite Icon;
        
        Models.ItemViewModel model;
        public Controller.ItensController itensController;
        private BoxCollider2D BoxCollider2D;

        private void Awake()
        {
            itensController = new Controller.ItensController();
            model = itensController.GetInitialData(id, Prefab, Icon);
            if (model == null)
            {
                Destroy(this.gameObject);
                return;
            }

            BoxCollider2D = GetComponent<BoxCollider2D>();
            transform.position = Utils.SetPositionZ(transform, BoxCollider2D.bounds.min.y);
        }

        public Models.ItemViewModel GetItemModel()
        {
            //if (DAL.ProjectVikingsContext.InventoryItens.Count >= InventoryView.space && DAL.ProjectVikingsContext.InventoryItens[0].ItemId != model.ItemId && DAL.ProjectVikingsContext.InventoryItens[1].ItemId != model.ItemId)
            //{
            //    print("Sem espaço no inventario");
            //    return null;
            //}
            //else
            //{
                Destroy(gameObject);
                return model;
            //}
        }
    }
}

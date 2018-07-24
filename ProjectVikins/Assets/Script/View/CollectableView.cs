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

        private void Awake()
        {
            itensController = new Controller.ItensController();
            model = itensController.HealthGetInitialData(transform.position);
            if (model == null)
            {
                Destroy(this.gameObject);
                return;
            }
            model.Prefab = Prefab;
            BoxCollider2D = GetComponent<BoxCollider2D>();
            transform.position = Utils.SetPositionZ(transform, BoxCollider2D.bounds.min.y);
        }

        public Models.HealthItemViewModel GetItemModel()
        {
            Destroy(gameObject);
            return model;
        }
    }
}

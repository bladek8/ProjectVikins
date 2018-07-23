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
        DAL.HealthItem data;
        public Controller.ItensController itensController;
        private BoxCollider2D BoxCollider2D;

        private void Awake()
        {
            itensController = new Controller.ItensController();
            data = itensController.HealthGetInitialData(transform.position);
            BoxCollider2D = GetComponent<BoxCollider2D>();
            transform.position = Utils.SetPositionZ(transform, BoxCollider2D.bounds.min.y);
        }

        public DAL.HealthItem GetItemModel()
        {
            Destroy(gameObject);
            return data;
        }
    }
}

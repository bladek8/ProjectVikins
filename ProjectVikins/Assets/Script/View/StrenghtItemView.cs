using Assets.Script.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.View
{
    public class StrenghtItemView : MonoBehaviour
    {
        Models.StrenghtItemViewModel model;
        public Controller.ItensController itensController;
        private BoxCollider2D BoxCollider2D;
        public GameObject Prefab;
        public Sprite Icon;

        private void Awake()
        {
            itensController = new Controller.ItensController();
            model = itensController.StrenghtGetInitialData(transform.position);
            if (model == null)
            {
                Destroy(this.gameObject);
                return;
            }
            model.Prefab = Prefab;
            model.Icon = Icon;
            BoxCollider2D = GetComponent<BoxCollider2D>();
            transform.position = Utils.SetPositionZ(transform, BoxCollider2D.bounds.min.y);
        }

        public Models.StrenghtItemViewModel GetStrenghtItemModel()
        {
            Destroy(gameObject);
            return model;
        }
    }
}

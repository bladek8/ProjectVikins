using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.View
{
    public class InventoryView : MonoBehaviour
    {
        public Image Inventary;

        private void Update()
        {

            if (Input.GetButtonDown("teste") && Inventary.enabled == false)
            {
                foreach (var item in DAL.ProjectVikingsContext.InventoryItens)
                {
                    if (item.Amount >= 1)
                    {
                        var obj = Instantiate(item.Prefab, new Vector3(0, 0, 99), Quaternion.identity);
                        obj.transform.parent = Inventary.transform;
                        var rectTrans = obj.GetComponent<RectTransform>();

                        var button = obj.GetComponent<Button>();
                        button.onClick.AddListener(delegate { Interacted(item.InventoryItemId); });
                    }
                }
                Inventary.enabled = true;
            }

            if (Inventary.enabled == true)
            {
                if (Input.GetButtonDown("Interaction"))
                {
                    Inventary.enabled = false;
                }
            }
        }

        public void Interacted(int inventoryItemId)
        {
            var inventoryItem = DAL.ProjectVikingsContext.InventoryItens.Where( x => x.InventoryItemId ==  inventoryItemId).First();

            if (inventoryItem.ItemTypeId == (int)DAL.ItemTypes.HealthItem)
            {
                var heatlhItem = DAL.ProjectVikingsContext.HealthItens.Where(x => x.ItemId == inventoryItem.ItemId).First();
                DAL.ProjectVikingsContext.playerModels.First().CurrentLife += heatlhItem.Health * heatlhItem.Amount;
            }

        }

    }
}

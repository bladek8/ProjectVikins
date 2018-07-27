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
        Button[] PrefabButtons;

        private void Update()
        {
            if (Input.GetButtonDown("teste") && Inventary.enabled == false)
            {
                foreach (var item in DAL.ProjectVikingsContext.InventoryItens)
                {
                    if (item.Amount >= 1 && item.ItemId == 1)
                    {
                        var obj = Instantiate(item.Prefab, new Vector3(0, 0, 99), Quaternion.identity);
                        obj.transform.SetParent(Inventary.transform);
                        var rectTrans = obj.GetComponent<RectTransform>();
                        rectTrans.anchoredPosition = new Vector2(0, 0);
                        rectTrans.localScale = new Vector2(0.75f, 0.75f);
                        var button = obj.GetComponent<Button>();
                        button.onClick.AddListener(delegate { Interacted(item.InventoryItemId); });
                    }
                    else if (item.Amount >= 1 && item.ItemId == 2)
                    {
                        var obj = Instantiate(item.Prefab, new Vector3(0, 0, 99), Quaternion.identity);
                        obj.transform.SetParent(Inventary.transform);
                        var rectTrans = obj.GetComponent<RectTransform>();
                        rectTrans.anchoredPosition = new Vector2(100, 0);
                        rectTrans.localScale = new Vector2(0.75f, 0.75f);
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
                    PrefabButtons = GetComponentsInChildren<Button>();
                    foreach (var buttons in PrefabButtons)
                    {
                        Destroy(buttons.gameObject);
                    }
                }
            }
        }

        public void Interacted(int inventoryItemId)
        {
            var inventoryItem = DAL.ProjectVikingsContext.InventoryItens.Where(x => x.InventoryItemId ==  inventoryItemId).First();

            if (inventoryItem.ItemTypeId == (int)DAL.ItemTypes.HealthItem)
            {
                var heatlhItem = DAL.ProjectVikingsContext.HealthItens.Where(x => x.ItemId == inventoryItem.ItemId).First();
                DAL.ProjectVikingsContext.playerModels.First().CurrentLife += heatlhItem.Health * heatlhItem.Amount;
            }
            else if (inventoryItem.ItemTypeId == (int)DAL.ItemTypes.StrenghtItem)
            {
                var strenghtItem = DAL.ProjectVikingsContext.StrenghtItens.Where(x => x.ItemId == inventoryItem.ItemId).First();
                DAL.ProjectVikingsContext.playerModels.First().AttackMin += strenghtItem.Strenght * strenghtItem.Amount;
                DAL.ProjectVikingsContext.playerModels.First().AttackMax += strenghtItem.Strenght * strenghtItem.Amount;
            }
        }
    }
}

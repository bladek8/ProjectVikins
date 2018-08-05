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
        public GameObject Inventary;
        public GameObject Title;
      
        List<Button> PrefabButtons;
        List<Image> IconSlots;
        List<Text> AmountTexts;

        private void Start()
        {
            IconSlots = new List<Image>();
            PrefabButtons = new List<Button>();
            AmountTexts = new List<Text>();
        }

        private void Update()
        {
            if (Input.GetButtonDown("teste"))
            {
                Inventary.SetActive(!Inventary.activeSelf);
                Title.SetActive(!Title.activeSelf);

                if (Inventary.activeSelf == true)
                {
                    if (PrefabButtons.Count <= 0)
                        PrefabButtons.AddRange(GetComponentsInChildren<Button>());
                    if (IconSlots.Count <= 0)
                        IconSlots.AddRange(Inventary.GetComponentsInChildren<Image>());
                    if (AmountTexts.Count <= 0)
                        AmountTexts.AddRange(Inventary.GetComponentsInChildren<Text>());

                    for (int i = 0; i < IconSlots.Count; i++)
                    {
                        if (IconSlots[i].name != "Icon")
                        {
                            IconSlots.Remove(IconSlots[i]);
                        }
                        if (IconSlots[i].name == "InventorySlot")
                            IconSlots.Remove(IconSlots[i]);
                    }

                    int j = 0;
                    foreach (var item in DAL.ProjectVikingsContext.InventoryItens)
                    {
                        IconSlots[j].sprite = item.Icon;
                        IconSlots[j].enabled = true;
                        PrefabButtons[j].interactable = true;
                        AmountTexts[j].enabled = true;
                        AmountTexts[j].text = item.Amount.ToString();
                        j++;
                    }
                    for (int i = j; i < IconSlots.Count; i++)
                    {
                        IconSlots[i].sprite = null;
                        IconSlots[i].enabled = false;
                        PrefabButtons[i].interactable = false;
                        AmountTexts[i].enabled = false;
                    }
                }
                
                //    else if (item.Amount >= 1 && item.ItemId == 2 && Inventary.activeSelf == true)
                //    {
                //        //var obj = Instantiate(item.Prefab, new Vector3(0, 0, 0), Quaternion.identity, InventarySlot.transform);
                //        //obj.transform.SetParent(transform);
                //        //var rectTrans = obj.GetComponent<RectTransform>();
                //        //rectTrans.anchoredPosition = new Vector3(0, 0, 0);
                //        //obj.transform.SetSiblingIndex(0);
                //        //var button = obj.GetComponent<Button>();
                //        //button.onClick.AddListener(delegate { Interacted(item.InventoryItemId); });
                //    }
                //}
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets.Script.View
{
    public class InventoryView : MonoBehaviour
    {
        #region Singleton
        public static InventoryView instance;
        private void Awake()
        {
            instance = this;
            if (instance != null)
            {
                Debug.LogWarning("more than 1 instance found");
            }
        }
        #endregion

        public GameObject Inventary;
        public GameObject Title;
        public GameObject Description;
        public Button DescriptionButton;
        public Text DescriptionText;
        public static int space = 2;

        List<Button> SlotButtons;
        List<Image> IconSlots;
        List<Text> AmountTexts;

        private void Start()
        {
            IconSlots = new List<Image>();
            SlotButtons = new List<Button>();
            AmountTexts = new List<Text>();
        }

        private void Update()
        {
            if (Input.GetButtonDown("teste") && !Input.GetKey(KeyCode.LeftShift))
            {
                Inventary.SetActive(!Inventary.activeSelf);
                Title.SetActive(!Title.activeSelf);

                if (Inventary.activeSelf == true)
                {
                    if (SlotButtons.Count <= 0)
                        SlotButtons.AddRange(GetComponentsInChildren<Button>());
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
                        SlotButtons[j].enabled = true;
                        AmountTexts[j].enabled = true;
                        AmountTexts[j].text = item.Amount.ToString();
                        var text = "texto qualquer " + j;
                        SlotButtons[j].onClick.AddListener(delegate { LeftClickInteraction(text); });
                        j++;
                    }
                    for (int i = j; i < IconSlots.Count; i++)
                    {
                        IconSlots[i].sprite = null;
                        IconSlots[i].enabled = false;
                        SlotButtons[i].enabled = false;
                        AmountTexts[i].enabled = false;
                    }
                    if (DAL.ProjectVikingsContext.InventoryItens.Count > 0)
                    {
                        if (DAL.ProjectVikingsContext.InventoryItens[0].ItemTypeId == 1)
                        {
                            DescriptionText.text = "Coco: Aumenta 5 de vida";
                        }
                        else
                        {
                            DescriptionText.text = "Coco Aberto: Aumenta 5 de força por 10 segundos";
                        }
                        EventSystem.current.SetSelectedGameObject(null);
                        SlotButtons[0].Select();
                    }
                }
                else
                {
                    Description.SetActive(false);
                }
            }
        }

        public void Interacted(int inventoryItemId)
        {
            var inventoryItem = DAL.ProjectVikingsContext.InventoryItens.Where(x => x.InventoryItemId == inventoryItemId).First();

            if (inventoryItem.ItemTypeId == (int)DAL.ItemTypes.HealthItem)
            {
                var heatlhItem = DAL.ProjectVikingsContext.HealthItens.Where(x => x.ItemId == inventoryItem.ItemId).First();
                DAL.ProjectVikingsContext.playerModels.First().CurrentLife += heatlhItem.Health * heatlhItem.Amount;
                print("Player usou o item de vida");
            }
            else if (inventoryItem.ItemTypeId == (int)DAL.ItemTypes.StrenghtItem)
            {
                var strenghtItem = DAL.ProjectVikingsContext.StrenghtItens.Where(x => x.ItemId == inventoryItem.ItemId).First();
                DAL.ProjectVikingsContext.playerModels.First().AttackMin += strenghtItem.Strenght * strenghtItem.Amount;
                DAL.ProjectVikingsContext.playerModels.First().AttackMax += strenghtItem.Strenght * strenghtItem.Amount;
                print("Player usou o item de força");
            }
        }

        public void LeftClickInteraction(string text)
        {
            EventSystem.current.SetSelectedGameObject(null);
            DescriptionButton.Select();
            DescriptionText.text = text;

        }
        public void DescriptionButtonOptions(bool isClickingButton1)
        {
            if (isClickingButton1)
            {
                foreach (var item in DAL.ProjectVikingsContext.InventoryItens)
                {
                    Interacted(item.InventoryItemId);
                }
                print("Equipping item/Using item");
            }
            else
            {
                print("Removing item");
            }
        }
    }
}

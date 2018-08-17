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
            if (instance != null)
            {
                Debug.LogWarning("more than 1 instance found");
            }
            instance = this;
        }
        #endregion
        
        public GameObject Inventary;
        public GameObject InventoryTab;
        public GameObject CharacterTab;
        public GameObject AbilityTreeTab;
        public GameObject SettingsTab;
        public GameObject Description;
        public GameObject EquipItem;
        public GameObject RemoveItem;
        public Text DescriptionText;
        public static int space = 2;

        List<Button> SlotButtons;
        List<Image> IconSlots;
        List<Text> AmountTexts;

        private void Start()
        {
            SlotButtons = new List<Button>();
            IconSlots = new List<Image>();
            AmountTexts = new List<Text>();
            SlotButtons.AddRange(Inventary.GetComponentsInChildren<Button>());
            IconSlots.AddRange(Inventary.GetComponentsInChildren<Image>().Where(x => x.name == "Icon"));
            AmountTexts.AddRange(Inventary.GetComponentsInChildren<Text>());
        }

        private void Update()
        {
            if (Input.GetButtonDown("teste") && !Input.GetKey(KeyCode.LeftShift))
            {
                Inventary.SetActive(!Inventary.activeSelf);
                InventoryTab.SetActive(!InventoryTab.activeSelf);
                CharacterTab.SetActive(!CharacterTab.activeSelf);
                AbilityTreeTab.SetActive(!AbilityTreeTab.activeSelf);
                SettingsTab.SetActive(!SettingsTab.activeSelf);

                if (Inventary.activeSelf == true)
                {                    
                    int j = 0;
                    foreach (var item in DAL.ProjectVikingsContext.InventoryItens)
                    {
                        IconSlots[j].sprite = item.Icon;
                        IconSlots[j].enabled = true;
                        SlotButtons[j].enabled = true;
                        AmountTexts[j].enabled = true;
                        AmountTexts[j].text = item.Amount.ToString();
                        var text = item.DescriptionText;
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
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = 1;
                    Description.SetActive(false);
                    EquipItem.SetActive(false);
                    RemoveItem.SetActive(false);
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

        public void LeftClickInteraction()
        {
            var image = EventSystem.current.currentSelectedGameObject.GetComponentsInChildren<Image>().Where(x => x.name == "Icon").First();

            if (image.sprite.name == "coco teste_0")
                DescriptionText.text = "Coco: Aumenta 5 de vida";
            else if (image.sprite.name == "coco teste_1")
                DescriptionText.text = "Coco Aberto: Aumenta 5 de força por 10 segundos";

            Description.SetActive(true);
            print("LeftClicked");
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Script.SystemManagement;

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

        public Image icon;
        public Image highlightImage;
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

        private BLL.InventoryItemFunctions inventoryItemFunctions = new BLL.InventoryItemFunctions();
        private BLL.ItemFunctions itemFunctions = new BLL.ItemFunctions();
        private BLL.ItemTypeFunctions itemTypeFunctions = new BLL.ItemTypeFunctions();
        private BLL.PlayerFunctions PlayerFunctions = new BLL.PlayerFunctions();

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
            if (Input.GetKeyDown(KeyCode.I) && !Input.GetKey(KeyCode.LeftShift))
            {
                Inventary.SetActive(!Inventary.activeSelf);
                InventoryTab.SetActive(!InventoryTab.activeSelf);
                CharacterTab.SetActive(!CharacterTab.activeSelf);
                AbilityTreeTab.SetActive(!AbilityTreeTab.activeSelf);
                SettingsTab.SetActive(!SettingsTab.activeSelf);

                if (Inventary.activeSelf == true)
                {
                    UpdateInventory();
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = 1;
                    highlightImage.enabled = false;
                    Description.SetActive(false);
                    EquipItem.SetActive(false);
                    RemoveItem.SetActive(false);
                }
            }
        }

        public void UpdateInventory()
        {
            int j = 0;
            foreach (var item in inventoryItemFunctions.GetData().Where(x => x.Amount > 0))
            {
                //arrumar questão dos Id's por serem iguais
                var _item = itemFunctions.GetModels().FirstOrDefault(x => x.ItemId == item.ItemId);
                SetValues(_item.Icon, true, j);
                AmountTexts[j].text = item.Amount.ToString();
                j++;
            }
            for (int i = j; i < IconSlots.Count; i++)
            {
                SetValues(null, false, j);
            }

            if (inventoryItemFunctions.GetData().Count > 0)
            {
                EventSystem.current.SetSelectedGameObject(null);
                SlotButtons[0].Select();
            }
        }
        
        public void Interacted(int? itemId)
        {
            if (!itemId.HasValue) return;
            
            var player = PlayerFunctions.GetModelById(PlayerFunctions.GetIdOfControllable());
            MonoBehaviour script;
            SystemManagement.SystemManagement.Scripts.TryGetValue(player.GameObject, out script);
            
            var itemAttr = itemFunctions.GetItemAtributtes(itemId.Value);
            script.CallMethod("UseItem", itemAttr);
            //_p.UseItem(itemAttr);

            //player.UseItem(itemAttr);            
            inventoryItemFunctions.DecreaseAmount(itemId);

            UpdateInventory();
        }

        public void LeftClickInteraction()
        {
            var image = EventSystem.current.currentSelectedGameObject.GetComponentsInChildren<Image>().SingleOrDefault(x => x.name == "Icon");

            var item = itemFunctions.GetDataByIcon(image.sprite);
            DescriptionText.text = item.Name + ": " + item.DescriptionText;
            Description.SetActive(true);
            print("LeftClicked");
        }

        public void DescriptionButtonOptions(bool isClickingButton1)
        {
            if (isClickingButton1)
            {
                var item = itemFunctions.GetDataByIcon(icon.sprite);
                Interacted(item.ItemId);
                EquipItem.SetActive(false);
                RemoveItem.SetActive(false);
                print("Equipping item/Using item");
            }
            else
            {
                print("Removing item");
            }
        }

        public void SetValues(Sprite sprite, bool enable, int j)
        {
            IconSlots[j].sprite = sprite;
            IconSlots[j].enabled = enable;
            SlotButtons[j].enabled = enable;
            AmountTexts[j].enabled = enable;
        }

    }
}

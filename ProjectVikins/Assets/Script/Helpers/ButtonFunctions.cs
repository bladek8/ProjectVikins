using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Script.Helpers
{
    public class ButtonFunctions : MonoBehaviour, IPointerClickHandler, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region Singleton
        public static ButtonFunctions instance;
        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning("more than one found");
                return;
            }
            instance = this;
        }
        #endregion

        public UnityEvent leftClick;
        public UnityEvent middleClick;
        public UnityEvent rightClick;
        public Image highlightImage;
        public Image Icon;
        public Button button;
        public GameObject equipItem;
        public GameObject removeItem;
        public Text equipItemText;
        public Text removeItemText;

        //private void Start()
        //{
        //    button = gameObject.GetComponent<Button>();
        //}

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                leftClick.Invoke();
            }
            if (eventData.button == PointerEventData.InputButton.Middle)
            {
                middleClick.Invoke();
            }
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                rightClick.Invoke();
            }
        }

        public void OnDeselect(BaseEventData eventData)
        {
            //eventData.selectedObject.GetComponent<Selectable>().Select();
            //if ((Input.mousePosition.x >= 0 && Input.mousePosition.x <= 808) || (Input.mousePosition.x >= 1190 && Input.mousePosition.x <= 1366) && (Input.mousePosition.y <= 768 && Input.mousePosition.y >= 82) || (Input.mousePosition.y <= 39 && Input.mousePosition.y >= 0))
            //{
            //    print("unselected");
            //}
            View.InventoryView.instance.Description.SetActive(false);
        }

        public void OnSelect(BaseEventData eventData) //Util para o joystick
        {
            //View.InventoryView.instance.Description.SetActive(true);
            //print("selected");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Icon.enabled == true)
            {
                highlightImage.enabled = true;
                if (button.name == "InventorySlot1")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-684, 123);
                else if (button.name == "InventorySlot2")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-584, 123);
                else if (button.name == "InventorySlot3")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-484, 123);
                else if (button.name == "InventorySlot4")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-384, 123);
                else if (button.name == "InventorySlot5")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-284, 123);
                else if (button.name == "InventorySlot6")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-184, 123);
                else if (button.name == "InventorySlot7")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-84, 123);
                else if (button.name == "InventorySlot8")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-684, 23);
                else if (button.name == "InventorySlot9")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-584, 23);
                else if (button.name == "InventorySlot10")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-484, 23);
                else if (button.name == "InventorySlot11")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-384, 23);
                else if (button.name == "InventorySlot12")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-284, 23);
                else if (button.name == "InventorySlot13")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-184, 23);
                else if (button.name == "InventorySlot14")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-84, 23);
                else if (button.name == "InventorySlot15")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-684, -77);
                else if (button.name == "InventorySlot16")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-584, -77);
                else if (button.name == "InventorySlot17")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-484, -77);
                else if (button.name == "InventorySlot18")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-384, -77);
                else if (button.name == "InventorySlot19")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-284, -77);
                else if (button.name == "InventorySlot20")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-184, -77);
                else if (button.name == "InventorySlot21")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-84, -77);
                else if (button.name == "InventorySlot22")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-684, -177);
                else if (button.name == "InventorySlot23")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-584, -177);
                else if (button.name == "InventorySlot24")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-484, -177);
                else if (button.name == "InventorySlot25")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-384, -177);
                else if (button.name == "InventorySlot26")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-284, -177);
                else if (button.name == "InventorySlot27")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-184, -177);
                else if (button.name == "InventorySlot28")
                    highlightImage.rectTransform.anchoredPosition = new Vector2(-84, -177);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
             highlightImage.enabled = false;
        }

        public void RightClickInteraction()
        {
            if (button.enabled == true)
            {
                equipItem.SetActive(true);
                removeItem.SetActive(true);
                if (button.name == "InventorySlot1")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-596.19f, 240);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-583.88f, 214.5799f);
                }
                else if (button.name == "InventorySlot2")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-496.19f, 240);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-483.88f, 214.5799f);

                }

                else if (button.name == "InventorySlot3")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-396.19f, 240);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-383.88f, 214.5799f);

                }

                else if (button.name == "InventorySlot4")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-296.19f, 240);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-283.88f, 214.5799f);

                }

                else if (button.name == "InventorySlot5")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-196.19f, 240);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-183.88f, 214.5799f);

                }
                else if (button.name == "InventorySlot6")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-96.19f, 240);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-83.88f, 214.5799f);

                }
                else if (button.name == "InventorySlot7")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(3.81f, 240);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(16.12f, 214.5799f);

                }

                else if (button.name == "InventorySlot8")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-596.19f, 140);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-583.88f, 114.5799f);

                }

                else if (button.name == "InventorySlot9")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-496.19f, 140);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-483.88f, 114.5799f);

                }

                else if (button.name == "InventorySlot10")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-396.19f, 140);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-383.88f, 114.5799f);

                }

                else if (button.name == "InventorySlot11")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-296.19f, 140);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-283.88f, 114.5799f);

                }

                else if (button.name == "InventorySlot12")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-196.19f, 140);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-183.88f, 114.5799f);

                }

                else if (button.name == "InventorySlot13")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-96.19f, 140);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-83.88f, 114.5799f);

                }

                else if (button.name == "InventorySlot14")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(3.81f, 140);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(16.12f, 114.5799f);

                }

                else if (button.name == "InventorySlot15")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-596.19f, 40);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-583.88f, 14.5799f);

                }

                else if (button.name == "InventorySlot16")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-496.19f, 40);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-483.88f, 14.5799f);

                }

                else if (button.name == "InventorySlot17")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-396.19f, 40);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-383.88f, 14.5799f);

                }

                else if (button.name == "InventorySlot18")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-296.19f, 40);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-283.88f, 14.5799f);

                }

                else if (button.name == "InventorySlot19")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-196.19f, 40);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-183.88f, 14.5799f);

                }

                else if (button.name == "InventorySlot20")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-96.19f, 40);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-83.88f, 14.5799f);

                }

                else if (button.name == "InventorySlot21")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(3.81f, 40);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(16.12f, 14.5799f);

                }

                else if (button.name == "InventorySlot22")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-596.19f, -60);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-583.88f, -85.4201f);

                }
                else if (button.name == "InventorySlot23")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-496.19f, -60);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-483.88f, -85.4201f);

                }

                else if (button.name == "InventorySlot24")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-396.19f, -60);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-383.88f, -85.4201f);
                }

                else if (button.name == "InventorySlot25")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-296.19f, -60);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-283.88f, -85.4201f);
                }

                else if (button.name == "InventorySlot26")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-196.19f, -60);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-183.88f, -85.4201f);
                }

                else if (button.name == "InventorySlot27")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(-96.19f, -60);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(-83.88f, -85.4201f);
                }

                else if (button.name == "InventorySlot28")
                {
                    equipItemText.rectTransform.anchoredPosition = new Vector2(3.81f, -60);
                    removeItemText.rectTransform.anchoredPosition = new Vector2(16.12f, -85.4201f);
                }
            }
        }
    }
}

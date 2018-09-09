using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;
using Assets.Script.View;
using System.Linq;

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
            //View.InventoryView.instance.Description.SetActive(false);
        }

        public void OnSelect(BaseEventData eventData) //Util para o joystick
        {
            //View.InventoryView.instance.Description.SetActive(true);
            //print("selected");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var _icon = eventData.pointerEnter.GetComponentsInChildren<Image>().FirstOrDefault(x => x.gameObject.layer == LayerMask.NameToLayer("Icon"));
            InventoryView.instance.icon = _icon;

            if (Icon.enabled == true)
            {
                highlightImage.enabled = true;

                for (int j = 1; j <= 4; j++)
                    for (int i = 1; i <= 7; i++)
                    {
                        var n = i + (7 * (j - 1));
                        if (button.name == "InventorySlot" + n)
                        {
                            highlightImage.rectTransform.anchoredPosition = new Vector2(-784 + (i * 100), 223 - (j * 100));
                            return;
                        }
                    }
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

                for (int j = 1; j <= 4; j++)
                    for (int i = 1; i <= 7; i++)
                    {
                        var n = i + (7 * (j - 1));
                        if (button.name == "InventorySlot" + n)
                        {
                            equipItemText.rectTransform.anchoredPosition = new Vector2(-696.19f + (i * 100), 240);
                            removeItemText.rectTransform.anchoredPosition = new Vector2(-683.88f + (i * 100), 214.5799f);
                            return;
                        }
                    }
            }
        }
    }
}

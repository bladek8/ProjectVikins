using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Script.Helpers
{
    public class ButtonFunctions : MonoBehaviour, /*IPointerClickHandler,*/ ISelectHandler, IDeselectHandler//, IPointerEnterHandler, IPointerExitHandler
    {
        #region Singleton
        //public static ButtonFunctions instance;
        //private void Awake()
        //{
        //    if (instance != null)
        //    {
        //        Debug.LogWarning("more than one found");
        //        return;
        //    }
        //    instance = this;
        //}
        #endregion

        //public UnityEvent leftClick;
        //public UnityEvent middleClick;
        //public UnityEvent rightClick;
        //public Image highlightImage;
        //public Image Icon;

        //public void OnPointerClick(PointerEventData eventData)
        //{
        //    if (eventData.button == PointerEventData.InputButton.Left)
        //    {
        //        leftClick.Invoke();
        //    }
        //    if (eventData.button == PointerEventData.InputButton.Middle)
        //    {
        //        middleClick.Invoke();
        //    }
        //    if (eventData.button == PointerEventData.InputButton.Right)
        //    {
        //        rightClick.Invoke();
        //    }
        //}

        public void OnDeselect(BaseEventData eventData)
        {
            if ((Input.mousePosition.x >= 0 && Input.mousePosition.x <= 808) || (Input.mousePosition.x >= 1190 && Input.mousePosition.x <= 1366) && (Input.mousePosition.y <= 768 && Input.mousePosition.y >= 82) || (Input.mousePosition.y <= 39 && Input.mousePosition.y >= 0))
            {
                View.InventoryView.instance.Description.SetActive(false);
                print("unselected");
            }
        }

        public void OnSelect(BaseEventData eventData)
        { 
            View.InventoryView.instance.Description.SetActive(true);
            print("selected");
        }

        //public void OnPointerEnter(PointerEventData eventData)
        //{
        //    if (Icon.enabled == true && Icon.sprite.name == "coco teste_0")
        //    {
        //        highlightImage.enabled = true;
        //        highlightImage.rectTransform.position = Input.mousePosition + new Vector3(40, -10, 0);
        //        highlightImage.sprite = View.CollectableView.DescriptionSpriteStatic;
        //    }
        //    else if (Icon.enabled == true && Icon.sprite.name == "coco teste_1")
        //    {
        //        highlightImage.enabled = true;
        //        highlightImage.rectTransform.position = Input.mousePosition + new Vector3(40, -10, 0);
        //        highlightImage.sprite = View.StrenghtItemView.DescriptionSpriteStatic;
        //    }
        //}

        //public void OnPointerExit(PointerEventData eventData)
        //{
        //    if (Icon.enabled == true && Icon.sprite.name == "coco teste_0")
        //    {
        //        highlightImage.enabled = false;
        //        print("exit highlight");
        //    }
        //}
    }
}

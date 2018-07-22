using UnityEngine;
using Assets.Script.Controller;
using Assets.Script.Models;
using UnityEngine.UI;
using System.Linq;

namespace Assets.Script.View
{
    public class SliderView : MonoBehaviour
    {
        public Slider LifeBar;
        public static RectTransform rect1;
        public static RectTransform rect2;
        public static RectTransform rect3;
        public static EnemyViewModel model = null;

        private void Start()
        {
            rect1 = GameObject.Find("Slider").GetComponent<RectTransform>();
            rect2 = GameObject.Find("Slider (1)").GetComponent<RectTransform>();
            rect3 = GameObject.Find("Slider (2)").GetComponent<RectTransform>();
        }

        private void FixedUpdate()
        {
            if (model == null || model.CurrentLife <= 0)
            {
                LifeBar.GetComponentsInChildren<Image>().ToList().ForEach(x => x.enabled = false);
            }
            else
            {
                LifeBar.GetComponentsInChildren<Image>().ToList().ForEach(x => x.enabled = true);
                LifeBar.value = (float)CalculateLife();
            }
        }

        float? CalculateLife()
        {
            return model.CurrentLife / model.MaxLife;
        }
    }
}

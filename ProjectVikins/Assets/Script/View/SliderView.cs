using UnityEngine;
using Assets.Script.Controller;
using Assets.Script.Models;
using UnityEngine.UI;
using System.Linq;

public class SliderView : MonoBehaviour
{
    public Slider LifeBar;
    public static EnemyViewModel model = null;

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

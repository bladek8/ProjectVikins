using UnityEngine;

public class CanvasView : MonoBehaviour
{
    public Canvas canvas;

    private void Awake()
    {
        RectTransform objectRectTransform = gameObject.GetComponent<RectTransform>();
        Debug.Log("width: " + objectRectTransform.rect.width + ", height: " + objectRectTransform.rect.height);
    }

    void Update()
    {

    }
}

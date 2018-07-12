using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaDetectionView : MonoBehaviour
{
    public UnityEvent EnterAreaDetectionTrigger;
    public UnityEvent OutAreaDetectionTrigger;
    BoxCollider2D[] boxColliders2D;
    public LayerMask playerMask;
    bool isInArea = false;

    private void Start()
    {
        boxColliders2D = gameObject.GetComponents<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        isInArea = false;
        foreach (var boxCollider in boxColliders2D)
        {
            if (Physics2D.OverlapBox(boxCollider.bounds.center, boxCollider.size, 0, playerMask))
                isInArea = true;
        }
        if (isInArea)
            EnterAreaDetectionTrigger.Invoke();
        else
            OutAreaDetectionTrigger.Invoke();
    }

}

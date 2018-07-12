using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaDetectionView : MonoBehaviour
{
    public UnityEvent EnterAreaDetectionTrigger;
    public UnityEvent OutAreaDetectionTrigger;
    BoxCollider2D boxCollider2D;
    public LayerMask playerMask;

    private void Start()
    {
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (Physics2D.OverlapBox(transform.position, boxCollider2D.size, 0, playerMask))
            EnterAreaDetectionTrigger.Invoke();
        else
            OutAreaDetectionTrigger.Invoke();
    }

}

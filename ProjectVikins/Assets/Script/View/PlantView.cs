using Assets.Script.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantView : MonoBehaviour
{

    private BoxCollider2D _boxCollider2D;
    private BoxCollider2D BoxCollider2D { get { return _boxCollider2D ?? (_boxCollider2D = GetComponent<BoxCollider2D>()); } }

    private Animator _animator;
    private Animator Animator { get { return _animator ?? (_animator = GetComponent<Animator>()); } }
    CountDown animatorCount;

    private void Start()
    {
        animatorCount = new CountDown(0.5);
    }

    void Update()
    {
        CountDown.DecreaseTime(animatorCount);
        if (animatorCount.ReturnedToZero)
            Animator.SetBool("Collided", false);

        transform.position = Utils.SetPositionZ(transform, BoxCollider2D.bounds.min.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger)
        {
            Animator.SetBool("Collided", true);
            animatorCount.StartToCount();
        }
    }
}

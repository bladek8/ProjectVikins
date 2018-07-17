using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Helpers;
using Assets.Script.Controller;
using System.Linq;

public class ArrowView : MonoBehaviour
{
    Animator _animator;
    Animator Animator { get { return _animator ?? (_animator = GetComponent<Animator>()); } }

    BoxCollider2D _boxCollider2D;
    BoxCollider2D BoxCollider2D { get { return _boxCollider2D ?? (_boxCollider2D = GetComponent<BoxCollider2D>()); } }


    [SerializeField] LayerMask target;
    [SerializeField] LayerMask NotCollideble;

    Vector3 direction;
    float distance;
    Vector3 startPosition;
    bool stop = false;
    [HideInInspector] public Vector2 mouseIn;
    CountDown destroyCountDown = new CountDown(5);
    [HideInInspector] public float holdTime;
    Vector3 boxColliderBoundMin;

    void Start()
    {
        distance = Vector2.Distance(mouseIn, transform.position);
        this.direction = new Vector3(mouseIn.x, mouseIn.y) - transform.position;
        startPosition = transform.position;
    }

    void Update()
    {
        boxColliderBoundMin = BoxCollider2D.bounds.min;
        CountDown.DecreaseTime(destroyCountDown);

        if (destroyCountDown.ReturnedToZero)
            Destroy(gameObject);

        if (!stop)
        {
            if (Vector3.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(startPosition.x, startPosition.y)) < distance * holdTime)
            {
                Animator.SetBool("Fly", true);
                transform.position = Vector3.MoveTowards(transform.position, transform.position + direction * (2 * holdTime), 4);
            }
            else
                Stop();

            Collider();
        }

        transform.position = Utils.SetPositionZ(transform, BoxCollider2D.bounds.min.y);
    }

    void Collider()
    {
        Debug.DrawLine(boxColliderBoundMin, BoxCollider2D.bounds.max, Color.blue, 0.5f);
        var hit = Physics2D.OverlapBox(boxColliderBoundMin, BoxCollider2D.bounds.max, 0, target);

        if (hit != null)
        {
            print("Acertou!");
            var script = hit.gameObject.GetComponent<MonoBehaviour>();
            script.SendMessage("GetDamage", 1);
            Stop();
        }
    }

    void Stop()
    {
        Animator.SetBool("Fly", false);
        Animator.SetBool("Collided", true);
        destroyCountDown.StartToCount();
        stop = true;
    }
}

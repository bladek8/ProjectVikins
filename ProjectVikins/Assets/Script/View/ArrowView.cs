using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Helpers;

public class ArrowView : MonoBehaviour
{
    Animator _animator;
    Animator Animator { get { return _animator ?? (_animator = GetComponent<Animator>()); } }

    BoxCollider2D _boxCollider2D;
    BoxCollider2D BoxCollider2D { get { return _boxCollider2D ?? (_boxCollider2D = GetComponent<BoxCollider2D>()); } }

    Vector3 direction;
    float distance;
    Vector3 startPosition;
    bool stop = false;
    public Vector2 mouseIn;
    CountDown destroyCountDown = new CountDown(5);
    public float holdTime;
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
        }

        transform.position = Utils.SetPositionZ(transform, BoxCollider2D.bounds.min.y);

        Collider();

    }
    
    void Collider()
    {
        Debug.DrawLine(boxColliderBoundMin, BoxCollider2D.bounds.max,Color.blue,0.5f);
        var hit = Physics2D.OverlapArea(boxColliderBoundMin, BoxCollider2D.bounds.max);
        if (hit != null)
        {
            if (hit.gameObject.tag == "Player" || hit.gameObject.tag == "Shottable" || hit.gameObject.tag == "Above")
                return;
            else if (hit.gameObject.tag == "Enemy")
            {
                var currentLife = hit.gameObject.GetComponent<Assets.Script.View.EnemyView>().model.Life -= 1;
                if (currentLife <= 0)
                {
                    MonoBehaviourAttributes.Destroy(hit.gameObject);
                    MonoBehaviourAttributes.Destroy(gameObject);
                }
            }

            if (!stop)
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

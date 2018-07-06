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
    public Vector2 mouseIn, mouseOut;

    CountDown destroyCountDown = new CountDown(5);

    void Start()
    {
        distance = Vector3.Distance(mouseIn, mouseOut);
        this.direction = new Vector3(mouseIn.x, mouseIn.y) - transform.position;
        startPosition = transform.position;
    }

    void Update()
    {
        if (destroyCountDown.ReturnedToZero)
            Destroy(gameObject);

        if (Vector3.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(startPosition.x, startPosition.y)) < distance && !stop)
        {
            Animator.SetBool("Fly", true);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, 100 * Time.deltaTime);
        }
        else if (!stop)
        {
            Animator.SetBool("Fly", false);
            Animator.SetBool("Collided", true);
            stop = true;
            destroyCountDown.StartToCount();
        }
        transform.position = Utils.SetPositionZ(transform, BoxCollider2D.bounds.min.y);
    }

    private void FixedUpdate()
    {
        CountDown.DecreaseTime(destroyCountDown);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Shottable")
            return;

        if (!stop)
        {
            stop = true;
            Animator.SetBool("Fly", false);
            Animator.SetBool("Collided", true);
            destroyCountDown.StartToCount();
        }
    }
}

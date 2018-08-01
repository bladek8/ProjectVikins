using Assets.Script.SystemManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Script.View
{
    public class WaterView : MonoBehaviour
    {
        public LayerMask water;

        PolygonCollider2D _PolygonCollider2D;
        private PolygonCollider2D PolygonCollider2D { get { return _PolygonCollider2D ?? (_PolygonCollider2D = GetComponent<PolygonCollider2D>()); } }
        SpriteRenderer _SpriteRenderer;
        private SpriteRenderer SpriteRenderer { get { return _SpriteRenderer ?? (_SpriteRenderer = GetComponent<SpriteRenderer>()); } }

        //Vector3 previousCollision = new Vector3();

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.isTrigger && (collision.tag == "Player" || collision.tag == "Enemy"))
            {
                var script = collision.GetComponent<MonoBehaviour>();

                print(PolygonCollider2D.Distance(collision).distance);

                script.CallMethod("InWater", Mathf.Abs(PolygonCollider2D.Distance(collision).distance));

                //Ray2D ray = new Ray2D(PolygonCollider2D.bounds.center, collision.gameObject.transform.position);
                //Debug.DrawLine(PolygonCollider2D.bounds.center, collision.gameObject.transform.position, Color.blue, 0.3f);
                //Debug.DrawLine(ray.GetPoint(0), ray.GetPoint(1), Color.green, 0.3f);
                //Debug.DrawLine(ray.GetPoint(0), ray.GetPoint(-1), Color.yellow, 0.3f);

                //print("center: " + PolygonCollider2D.bounds.center + " | Ray Center: " + ray.origin);
                //print("destination: " + collision.gameObject.transform.position + " | Ray destination: " + ray.direction);

                //for (float i = 1; i < (SpriteRenderer.size.x > SpriteRenderer.size.y? SpriteRenderer.size.x : SpriteRenderer.size.y); i += 0.5f)
                //{
                //    Vector2 point = ray.GetPoint(i);
                //    //Debug.DrawLine(PolygonCollider2D.bounds.center, point, Color.red, 0.3f);
                //    //Debug.DrawLine(PolygonCollider2D.bounds.center, ray.GetPoint(-1), Color.black, 0.3f);

                //    var hit = Physics2D.OverlapPoint(point);

                //    if (hit == false && previousCollision != null)
                //    {

                //        //print(previousCollision);
                //        script.CallMethod("InWater", Mathf.Abs(Vector2.Distance(PolygonCollider2D.bounds.center, collision.gameObject.GetComponent<SpriteRenderer>().bounds.min) - Vector2.Distance(PolygonCollider2D.bounds.center, point)));
                //        previousCollision = new Vector3();
                //        return;
                //    }
                //    previousCollision = point;
                //}

                //var hits = Physics2D.RaycastAll(PolygonCollider2D.bounds.center, collision.transform.position, SpriteRenderer.size.x > SpriteRenderer.size.y ? SpriteRenderer.size.x : SpriteRenderer.size.y);

            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player" || collision.tag == "Enemy")
            {
                var script = collision.GetComponent<MonoBehaviour>();

                script.CallMethod("OutWater");
            }
        }
    }
}

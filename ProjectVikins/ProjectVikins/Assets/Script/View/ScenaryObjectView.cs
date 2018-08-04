using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Helpers;
using UnityEngine;

namespace Assets.Script.View
{
    public class ScenaryObjectView : MonoBehaviour
    {
        private BoxCollider2D _boxCollider2D;
        private BoxCollider2D BoxCollider2D { get { return _boxCollider2D ?? (_boxCollider2D = GetComponent<BoxCollider2D>()); } }

        private void Update()
        {
            transform.position = Utils.SetPositionZ(transform, BoxCollider2D.bounds.min.y);
        }
    }
}

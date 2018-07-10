using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Script.Helpers;

namespace Assets.Script.View
{
    public class NPCView : MonoBehaviour
    {
        BoxCollider2D colliderTransform;

        private void Start()
        {
            colliderTransform = GetComponents<BoxCollider2D>().Where(x => x.isTrigger == false).First();
        }
        public void Interaction()
        {
            Debug.Log("Interacted with NPC!");
        }

        private void FixedUpdate()
        {

            transform.position = Utils.SetPositionZ(transform, colliderTransform.bounds.min.y);
        }
    }
}

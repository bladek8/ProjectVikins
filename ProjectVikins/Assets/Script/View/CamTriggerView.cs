using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine;

namespace Assets.Script.View
{
    public class CamTriggerView : MonoBehaviour
    {
        public UnityEvent cameraTrigger;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
                cameraTrigger.Invoke();
        }
    }      
}

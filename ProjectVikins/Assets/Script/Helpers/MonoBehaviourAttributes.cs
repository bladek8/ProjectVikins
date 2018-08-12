using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Helpers
{
    public class MonoBehaviourAttributes : MonoBehaviour
    {
        public static void DestroyGameObject(GameObject ob)
        {
            Destroy(ob);
        }
        public static void StartCorrotine(IEnumerator enumerator)
        {
            StartCorrotine(enumerator);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Helpers
{
    class MonoBehaviourAttributes : MonoBehaviour
    {
        static void DestroyGameObject(GameObject ob)
        {
            Destroy(ob);
        }
    }
}

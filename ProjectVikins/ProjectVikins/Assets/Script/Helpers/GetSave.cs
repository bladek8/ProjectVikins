using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Helpers
{
    public class GetSave : MonoBehaviour
    {
        private void Awake()
        {
            DAL.MVC_Game2Context.GetSave();
        }
    }
}

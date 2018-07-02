using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Controller
{
    public class CameraController
    {
        private readonly BLL.PlayerFunctions PlayerFunctions = new BLL.PlayerFunctions();

        public CameraController()
        {
        }

        public Transform UpdatePlayerTranform()
        {
            return PlayerFunctions.GetData().Single(x => x.IsBeingControllable).Transform;
        }
    }
}

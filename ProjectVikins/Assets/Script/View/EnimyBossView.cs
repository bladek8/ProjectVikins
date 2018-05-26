using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.View
{
    public class EnimyBossView : MonoBehaviour
    {
        private Controller.EnimyBossController enimyBossController;

        private void Start()
        {
            enimyBossController = new Controller.EnimyBossController(this.GetInstanceID(),100, 3 , 5, 10, 10, 20);
            enimyBossController.MudarVelocidade(50);
        }


    }
}

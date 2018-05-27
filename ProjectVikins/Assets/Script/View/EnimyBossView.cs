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
            enimyBossController = new Controller.EnimyBossController(new Script.DAL.EnimyBoss { EnimyBossId = this.GetInstanceID(), Life = 100, CharacterTypeId = 3, SpeedWalk = 5, SpeedRun = 10, AttackMin = 10, AttackMax = 20 });
        }


    }
}

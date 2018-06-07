using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.View
{
    class EnemyAssassinView : MonoBehaviour
    {
        private Controller.EnemyAssassinController enemyAssassinController;
        [SerializeField] int life;
        [SerializeField] int speedWalk;
        [SerializeField] int speedRun;
        [SerializeField] int attackMax;
        [SerializeField] int attackMin;


        private void Start()
        {
            enemyAssassinController = new Controller.EnemyAssassinController(new Models.EnemyAssassinViewModel() {Life = life, SpeedWalk = speedWalk, AttackMax = attackMax, AttackMin = attackMin, SpeedRun = speedRun, CharacterTypeId = 1, EnemyAssassinId = gameObject.GetInstanceID() });
        }
    }
}

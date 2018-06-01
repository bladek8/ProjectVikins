using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.View
{
    public class EnimyView : MonoBehaviour
    {
        Controller.EnimyController enimyController;
        [SerializeField] GameObject gObject;
        LayerMask targetMask;

        FieldOfView fow;

        void Start()
        {
            enimyController = new Controller.EnimyController( new DAL.Enimy { EnimyId = this.gameObject.GetInstanceID(), Life = 2, CharacterTypeId = 5, SpeedRun = 3, SpeedWalk = 3, AttackMin = 1, AttackMax = 1 });

            fow = gObject.GetComponent<FieldOfView>();
        }

        private void FixedUpdate()
        {
            enimyController.followPlayer.DecreaseTime(enimyController.followPlayer);
            if (fow.visibleTargets.Count > 0)
            {
                enimyController.WalkTowardTo(fow, transform);
            }
            else
                enimyController.target = null;
        }

        //public void TakeDamage(int? damage)
        //{
        //    if (Convert.ToInt32(enimyController.DecreaseStats("Life", damage, this.GetInstanceID())) <= 0)
        //        Destroy(this.gameObject);
        //}
    }
}

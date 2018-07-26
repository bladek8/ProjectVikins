using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Script.Helpers;

namespace Assets.Script.View
{
    public class EnemyWarriorView : EnemyView
    {
        private void Awake()
        {
            SystemManagement.SystemManagement.EnemyWarriorView.Add(this);
        }

        private void FixedUpdate()
        {
            if (disabledCountDown.CoolDown > 0)
            {
                CountDown.DecreaseTime(disabledCountDown);
                return;
            }

            if (model.IsDead)
                return;

            EnemyUpdate();

            if (enemyController.fow.visibleTargets.Count > 0)
            {
                enemyController.FindTarget(transform);

                if (enemyController.target != null)
                {
                    if (Vector2.Distance(transform.position, enemyController.target.position) > 0.5f)
                    {
                        enemyController.WalkTowardTo(transform, ref model);
                        EnemyAnimator.SetBool("isWalking", true);
                    }
                    else
                        enemyController.canAttack = true;
                }
                else
                    EnemyAnimator.SetBool("isWalking", false);
            }
            else
            {
                EnemyAnimator.SetBool("isWalking", false);
                enemyController.target = null;
                enemyController.canAttack = false;
            }

            if (attackCountDown.CoolDown > 0 && enemyController.canAttack)
            {
                enemyController.Attack(transform, BoxCollider2D.size, playerLayer);
            }


            if (attackCountDown.CoolDown <= 0)
            {
                enemyController.targetsAttacked.Clear();
                if (enemyController.canAttack)
                    attackCountDown.StartToCount();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.View
{
    public class EnemyArcherView : EnemyView
    {
        [SerializeField] GameObject Arrow;
        Vector2 mouseIn;

        Vector2 startYRange = new Vector2(-1, 1);
        Vector2 startXRange = new Vector2(-1, 1);
        Vector2 YRange;
        Vector2 XRange;

        Transform oldTarget = null;

        private void Awake()
        {
            YRange = startYRange;
            XRange = startXRange;
        }

        private void FixedUpdate()
        {
            if (model.IsDead)
                return;

            if (oldTarget != enemyController.target)
            {
                YRange = startYRange;
                XRange = startXRange;
            }

            if (enemyController.target != null)
                oldTarget = enemyController.target;

            EnemyUpdate();

            if (enemyController.fow.visibleTargets.Count > 0)
            {
                enemyController.FindTarget(transform);

                if (enemyController.target != null)
                {
                    if (Vector2.Distance(transform.position, enemyController.target.position) > 4)
                    {
                        enemyController.WalkTowardTo(transform, ref model);
                        EnemyAnimator.SetBool("isWalking", true);
                    }
                    else
                    {
                        enemyController.canAttack = true;
                        EnemyAnimator.SetBool("isWalking", false);
                    }
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

            if (attackCountDown.CoolDown <= 0)
            {
                enemyController.targetsAttacked.Clear();

                if (attackCountDown.ReturnedToZero)
                {
                    model.SpeedRun = model.SpeedRun * 2;
                    model.SpeedWalk = model.SpeedWalk * 2;
                }

                if (enemyController.canAttack)
                {
                    model.SpeedRun = model.SpeedRun / 2;
                    model.SpeedWalk = model.SpeedWalk / 2;
                    attackCountDown.StartToCount();

                    if (enemyController.target != null)
                        Shoot();
                    enemyController.canAttack = false;
                }
            }
        }

        public void Shoot()
        {
            var randomX = UnityEngine.Random.Range(XRange.x, XRange.y);
            var randomY = UnityEngine.Random.Range(YRange.x, YRange.y);

            SetMinManRange(randomX, "X");
            SetMinManRange(randomY, "Y");

            mouseIn = new Vector2(enemyController.target.position.x + randomX, enemyController.target.position.y + randomY);
            var vectorDirection = mouseIn - new Vector2(transform.position.x, transform.position.y);
            var degrees = (Mathf.Atan2(vectorDirection.y, vectorDirection.x) * Mathf.Rad2Deg) - 90;
            if (degrees < 0f) degrees += 360f;

            var arrow = Instantiate(Arrow, new Vector3(transform.position.x, transform.position.y, -80), Quaternion.Euler(0, 0, degrees));
            var script = arrow.GetComponent<ArrowView>();
            script.mouseIn = mouseIn;
            script.holdTime = 1;
        }

        public void SetMinManRange(float value, string range)
        {
            if (range == "Y")
            {
                if (value > 0 && value < YRange.y)
                    YRange.y = value;
                if (value < 0 && value > YRange.x)
                    YRange.x = value;
            }
            if (range == "X")
            {
                if (value > 0 && value < XRange.y)
                    XRange.y = value;
                if (value < 0 && value > XRange.x)
                    XRange.x = value;
            }
        }

    }
}

using Assets.Script.Controller;
using Assets.Script.Helpers;
using Assets.Script.View.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.View
{
    public class ArcherView : _Character
    {
        Helpers.CountDown attackCountDown = new Helpers.CountDown(1f);

        [SerializeField] GameObject Arrow;
        Vector2 mouseIn;
        Counter counter = new Counter();

        private void FixedUpdate()
        {
            CharacterUpdate();

            CountDown.DecreaseTime(attackCountDown);

            if (isPlayable)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    mouseIn = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                    Counter.Count(counter);
                }
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    var vectorDirection = mouseIn - new Vector2(transform.position.x, transform.position.y);
                    var degrees = (Mathf.Atan2(vectorDirection.y, vectorDirection.x) * Mathf.Rad2Deg) - 90;
                    if (degrees < 0f) degrees += 360f;

                    var arrow = Instantiate(Arrow, new Vector3(transform.position.x, transform.position.y, -80), Quaternion.Euler(0, 0, degrees));
                    var script = arrow.GetComponent<ArrowView>();
                    script.mouseIn = mouseIn;
                    script.holdTime = counter.Time;
                    counter.ResetCounter();
                }
            }
            else
            {
                if (model.PlayerMode == PlayerModes.Attack)
                {

                    if (playerController.fow.visibleTargets.Count > 0)
                    {
                        playerController.FindTarget(transform);

                        if (playerController.target != null)
                        {
                            playerController.canAttack = true;
                        }
                    }
                    else
                    {
                        playerController.target = null;
                        playerController.canAttack = false;
                    }
                }

                if (attackCountDown.CoolDown <= 0)
                {
                    playerController.targetsAttacked.Clear();

                    if (attackCountDown.ReturnedToZero)
                    {
                        model.SpeedRun = model.SpeedRun * 2;
                        model.SpeedWalk = model.SpeedWalk * 2;
                    }

                    if (playerController.canAttack)
                    {
                        model.SpeedRun = model.SpeedRun / 2;
                        model.SpeedWalk = model.SpeedWalk / 2;
                        attackCountDown.StartToCount();
                        Shoot();
                        playerController.canAttack = false;
                    }
                }
            }
        }

        public void Shoot()
        {
            mouseIn = new Vector2(playerController.target.position.x, playerController.target.position.y);
            var vectorDirection = mouseIn - new Vector2(transform.position.x, transform.position.y);
            var degrees = (Mathf.Atan2(vectorDirection.y, vectorDirection.x) * Mathf.Rad2Deg) - 90;
            if (degrees < 0f) degrees += 360f;

            var arrow = Instantiate(Arrow, new Vector3(transform.position.x, transform.position.y, -80), Quaternion.Euler(0, 0, degrees));
            var script = arrow.GetComponent<ArrowView>();
            script.mouseIn = mouseIn;
            script.holdTime = 1;
        }

    }
}

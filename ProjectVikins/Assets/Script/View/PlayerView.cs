using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Script.Controller;
using Assets.Script.Helpers;
using Assets.Script.View.Shared;

namespace Assets.Script.View
{
    public class PlayerView : _Character
    {
        Helpers.CountDown attackCountDown = new Helpers.CountDown(1.5);

        private void FixedUpdate()
        {
            CharacterUpdate();

            CountDown.DecreaseTime(attackCountDown);

            if (isPlayable)
            {
                #region Attack

                if (attackCountDown.CoolDown <= 0)
                {
                    playerController.targetsAttacked.Clear();

                    if (attackCountDown.ReturnedToZero)
                    {
                        model.SpeedRun = model.SpeedWalk * 2;
                        model.SpeedWalk = model.SpeedWalk * 2;
                    }
                    if (Input.GetKey(KeyCode.L))
                    {
                        //colocar todos player em modo de attack
                        model.PlayerMode = PlayerModes.Attack;
                        //playerController

                        model.SpeedRun = model.SpeedWalk / 2;
                        model.SpeedWalk = model.SpeedWalk / 2;
                        attackCountDown.StartToCount();
                    }
                }
                else
                    playerController.Attack(transform, PlayerBoxCollider2D.size);

                #endregion
            }
            else
            {
                #region Attack

                if (model.PlayerMode == PlayerModes.Attack)
                {
                    if (playerController.fow.visibleTargets.Count > 0)
                    {
                        playerController.FindTarget(transform);

                        if (playerController.target != null)
                        {
                            if (Mathf.Abs(Vector3.Distance(transform.position, playerController.target.position)) > 0.5)
                            {
                                playerController.WalkTowardTo(transform, ref model);
                                PlayerAnimator.SetBool("isWalking", true);
                            }
                            else
                                playerController.canAttack = true;
                        }
                        else
                            PlayerAnimator.SetBool("isWalking", false);
                    }
                    else
                    {
                        PlayerAnimator.SetBool("isWalking", false);
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
                    }

                }
                else if (playerController.canAttack)
                {
                    playerController.Attack(transform, PlayerBoxCollider2D.size);
                }

                #endregion
            }
        }
    }
}

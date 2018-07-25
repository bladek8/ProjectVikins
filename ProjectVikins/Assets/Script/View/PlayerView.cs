using UnityEngine;
using Assets.Script.Helpers;
using Assets.Script.View.Shared;

namespace Assets.Script.View
{
    public class PlayerView : _Character
    {
        CountDown attackCountDown = new CountDown(1.5);

        private void FixedUpdate()
        {
            if (model.IsDead)
                return;

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
                        playerController.AttackMode();

                        model.SpeedRun = model.SpeedWalk / 2;
                        model.SpeedWalk = model.SpeedWalk / 2;
                        attackCountDown.StartToCount();
                    }
                }
                else
                    playerController.Attack(transform, PlayerCollider2D.size);

                #endregion

                if (Input.GetKey(KeyCode.P))
                {
                    playerController.Defend(ref model);
                }
                if (Input.GetKeyUp(KeyCode.P))
                {
                    model.DirectionsDefended = null;
                }
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
                        if (Mathf.Abs(Vector3.Distance(transform.position, cv.playerGameObj.transform.position)) > DistanceOfPlayer)
                        {
                            playerController.WalkToPlayer(transform, cv.playerGameObj.transform, ref model);
                            PlayerAnimator.SetBool("isWalking", true);
                        }
                        else
                        {
                            PlayerAnimator.SetBool("isWalking", false);
                            PlayerAnimator.SetBool("isRunning", false);
                        }

                        input = playerController.GetInput();
                        PlayerSpriteRenderer.flipX = input.Flip.Value;
                        PlayerAnimator.SetFloat("speedX", input.Vector2.x);
                        PlayerAnimator.SetFloat("speedY", input.Vector2.y);

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
                else if (playerController.canAttack && playerController.target != null)
                {
                    playerController.Attack(transform, PlayerCollider2D.size);
                }

                #endregion
            }
        }
    }
}

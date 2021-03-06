﻿using Assets.Script.Controller;
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

        Vector2 startYRange = new Vector2(-1, 1);
        Vector2 startXRange = new Vector2(-1, 1);
        Vector2 YRange;
        Vector2 XRange;

        bool hadVelocityDecrease = false;
        Transform _oldTarget = null;

        private void Awake()
        {
            YRange = startYRange;
            XRange = startXRange;
            SystemManagement.SystemManagement.Scripts.Add(gameObject, this);
        }
        
        private void FixedUpdate()
        {

            PlayerSpriteRenderer.material.SetFloat("_BottomLimit", transform.position.y - halfSizeY + distanceCenterWater);
            
            if (disabledCountDown.CoolDown > 0)
            {
                CountDown.DecreaseTime(disabledCountDown);
                return;
            }

            if (model.IsDead)
                return;

            if (_oldTarget != playerController.target)
            {
                YRange = startYRange;
                XRange = startXRange;
            }

            _oldTarget = playerController.target;

            CharacterUpdate();

            CountDown.DecreaseTime(attackCountDown);
            
            if (isPlayable)
            {
                if (attackCountDown.CoolDown <= 0)
                {
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        if (!hadVelocityDecrease)
                        {
                            model.SpeedRun = model.SpeedRun / 2;
                            model.SpeedWalk = model.SpeedWalk / 2;
                            hadVelocityDecrease = true;
                        }
                        Counter.Count(counter);
                    }
                    if (Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        mouseIn = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                        Shoot(counter.Time, true);
                        counter.ResetCounter();
                        playerController.AttackMode();
                        model.SpeedRun = model.SpeedRun * 2;
                        model.SpeedWalk = model.SpeedWalk * 2;
                        attackCountDown.StartToCount();
                            hadVelocityDecrease = false;
                    }
                }
            }
            else
            {
                if (attackCountDown.ReturnedToZero)
                {
                    model.SpeedRun = model.SpeedRun * 2;
                    model.SpeedWalk = model.SpeedWalk * 2;
                }

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

                        input = playerController.GetInput(model.LastMoviment.Value);
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

                    if (playerController.canAttack && playerController.target != null)
                    {
                        model.SpeedRun = model.SpeedRun / 2;
                        model.SpeedWalk = model.SpeedWalk / 2;

                        attackCountDown.StartToCount();

                        var randomX = UnityEngine.Random.Range(XRange.x, XRange.y);
                        var randomY = UnityEngine.Random.Range(YRange.x, YRange.y);
                        SetMinManRange(randomX, "X");
                        SetMinManRange(randomY, "Y");
                        mouseIn = new Vector2(playerController.target.position.x + randomX, playerController.target.position.y + randomY);

                        Shoot(1);
                        playerController.canAttack = false;
                    }
                }
            }
        }

        public void Shoot(float holdTime, bool showSliderEnemy = false)
        {

            var vectorDirection = mouseIn - new Vector2(transform.position.x, transform.position.y);
            var degrees = (Mathf.Atan2(vectorDirection.y, vectorDirection.x) * Mathf.Rad2Deg) - 90;
            if (degrees < 0f) degrees += 360f;

            var arrow = Instantiate(Arrow, new Vector3(transform.position.x, transform.position.y, -80), Quaternion.Euler(0, 0, degrees));
            var script = arrow.GetComponent<ArrowView>();
            script.mouseIn = mouseIn;
            script.holdTime = holdTime;
            script.showSliderEnemy = showSliderEnemy;
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

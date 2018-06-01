﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Script.Helpers;
using Assets.Script.Models;

namespace Assets.Script.Controller
{
    public class EnimyController : Shared.CharacterController<Models.EnimyViewModel>
    {
        private readonly BLL.EnimyFunctions enimyFunctions;
        private readonly int id;
        List<Transform> players;
        private Utils utils = new Utils();

        public CountDown followPlayer = new CountDown();
        public Transform target;

        public EnimyController(DAL.Enimy model)
            : base("EnimyFunctions")
        {
            enimyFunctions = new BLL.EnimyFunctions();
            enimyFunctions.Create(model);
            this.id = model.EnimyId;
            players = utils.GetTransformInLayer("Player");
        }


        public void WalkTowardTo(FieldOfView fow, Transform _transform)
        {
            if (target != null && followPlayer.CoolDown <= 0 || target == null)
            {
                followPlayer.CoolDown = followPlayer.Rate;
                if (target == null)
                    target = utils.NearTargetInView(players, fow.visibleTargets, _transform);
                else
                {
                    target = utils.NearTarget(players, _transform, target);
                    fow.TurnView(target);
                }
            }


            if (fow.visibleTargets.Contains(target))
            {
                if (Vector3.Distance(_transform.position, target.transform.position) > 0)
                {
                    _transform.position = Vector3.MoveTowards(_transform.position, target.transform.position, enimyFunctions.GetDataById(id).SpeedWalk * Time.deltaTime);
                }
                fow.TurnView(target);
            }
            else
                target = null;
        }

        public override int GetDamage()
        {
            throw new NotImplementedException();
        }

        public override void UpdateStats(EnimyViewModel model)
        {
            throw new NotImplementedException();
        }

        public override void Decrease(EnimyViewModel model)
        {
            throw new NotImplementedException();
        }

        public override void Increase(EnimyViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
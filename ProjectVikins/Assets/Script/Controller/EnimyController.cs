using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Script.Helpers;

namespace Assets.Script.Controller
{
    public class EnimyController : Shared.CharacterController<BLL.EnimyFunctions>
    {
        private readonly BLL.EnimyFunctions enimyFunctions;
        private readonly int id;
        List<Transform> players;
        private Utils utils = new Utils();

        public Models.Shared.CountDown followPlayer = new Models.Shared.CountDown();
        public Transform target;

        public EnimyController(DAL.Enimy model)
        {
            enimyFunctions = new BLL.EnimyFunctions(model);
            this.id = model.EnimyId;
            players = utils.GetTransformInLayer("Player");
            SetFunction();
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

        public override void SetFunction()
        {
            this._TFuncitons = enimyFunctions;
        }
        //public void DecreaseStatus(string stats, object value, int id)
        //{
        //    Debug.Log(_TFuncitons);
        //    enimyFunctions.DecreaseStats(stats, value, id);
        //    Debug.Log(enimyFunctions.GetDataById(id).Life);
        //}
    }
}
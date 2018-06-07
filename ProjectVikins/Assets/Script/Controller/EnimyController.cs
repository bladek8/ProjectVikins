using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Script.Helpers;
using Assets.Script.Models;

namespace Assets.Script.Controller
{
    public class EnimyController : Shared._CharacterController<Models.EnimyViewModel>
    {
        private readonly BLL.EnimyFunctions enimyFunctions;
        private readonly int id;
        List<Transform> players;
        private Utils utils = new Utils();
        public bool canAttack;
        public FieldOfView fow;

        public CountDown followPlayer = new CountDown();
        public Transform target;

        public EnimyController(Models.EnimyViewModel model)
        {
            enimyFunctions = new BLL.EnimyFunctions();
            enimyFunctions.Create(model);
            this.id = model.EnimyId;
            players = utils.GetTransformInLayer("Player");
        }

        public void WalkTowardTo(Transform _transform)
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
                if (target == null) return;
                _transform.position = Vector3.MoveTowards(_transform.position, target.transform.position, enimyFunctions.GetDataById(id).SpeedWalk * Time.deltaTime);
                fow.TurnView(target);
                if (Math.Abs(Vector3.Distance(target.transform.position, _transform.position)) < 1f)
                    canAttack = true;
                else canAttack = false;
            }
            else
            {
                canAttack = false;
                target = null;
            }
        }

        public Helpers.PossibleMoviment GetDirection(Transform transform)
        {
            var vectorDirection = target.transform.position - transform.position;
            var degrees = Mathf.Atan2(vectorDirection.y, vectorDirection.x) * Mathf.Rad2Deg;
            var position = (int)((Mathf.Round(degrees / 45f) + 8) % 8);

            switch (position)
            {
                case 0:
                    return Helpers.PossibleMoviment.Right;
                case 1:
                    return Helpers.PossibleMoviment.Up_Right;
                case 2:
                    return Helpers.PossibleMoviment.Up;
                case 3:
                    return Helpers.PossibleMoviment.Up_Left;
                case 4:
                    return Helpers.PossibleMoviment.Left;
                case 5:
                    return Helpers.PossibleMoviment.Down_Left;
                case 6:
                    return Helpers.PossibleMoviment.Down;
                case 7:
                    return Helpers.PossibleMoviment.Down_Right;
                default:
                    return Helpers.PossibleMoviment.None;
            }
        }

        public override int GetDamage()
        {
            var player = enimyFunctions.GetDataById(id);
            return rnd.Next(player.AttackMin, player.AttackMax);
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

        public override void Attack(Transform transform, Vector3 size, LayerMask targetLayer)
        {
            var hitColliders = Physics2D.OverlapBoxAll(PositionCenterAttack(size, transform), size, 90f, targetLayer);
            foreach (var hitCollider in hitColliders)
            {

                if (targetsAttacked.Contains(hitCollider.gameObject.GetInstanceID())) continue;
                targetsAttacked.Add(hitCollider.gameObject.GetInstanceID());

                if (Convert.ToInt32(DecreaseStats(hitCollider.gameObject.gameObject.name, "Life", GetDamage(), hitCollider.gameObject.GetInstanceID())) <= 0)
                {
                    fow.visibleTargets.Remove(hitCollider.transform);
                    players.Remove(hitCollider.transform);
                    Destroy(hitCollider.gameObject);
                }
            }
        }

        public override Vector3 PositionCenterAttack(Vector3 colSize, Transform transform)
        {
            var player = enimyFunctions.GetDataById(id);
            return transform.position + PositionAttack(colSize, GetDirection(transform));
        }

        public void SetFieldOfView(FieldOfView fow)
        {
            this.fow = fow;
        }
    }
}
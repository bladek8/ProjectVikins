using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Assets.Script.Helpers;
using Assets.Script.Models;
using Assets.Script.BLL;

namespace Assets.Script.Controller
{

    class _Transform
    {
        public Transform Transform { get; set; }
    }

    public class EnemyController : Shared._CharacterController<Models.EnemyViewModel>
    {
        private readonly BLL.EnemyFunctions enemyFunctions = new BLL.EnemyFunctions();

        private int id;
        private Utils utils = new Utils();
        public bool canAttack;
        public FieldOfView fow;

        public CountDown followPlayer = new CountDown();
        public Transform target;

        public EnemyController()
        {
        }

        public void WalkTowardTo(Transform _transform, ref EnemyViewModel model)
        {
            if (fow.visibleTargets.Contains(target))
            {
                if (target == null) return;
                _transform.position = Vector3.MoveTowards(_transform.position, target.transform.position, enemyFunctions.GetModelById(id).SpeedWalk.Value * Time.deltaTime);
                fow.TurnView(target);
                model.LastMoviment = GetDirection(_transform.position, target.position);
                canAttack = false;
            }
            else
            {
                canAttack = false;
                target = null;
            }
        }

        public void FindTarget(Transform _transform)
        {
            if (target != null && followPlayer.CoolDown <= 0 || target == null)
            {
                followPlayer.StartToCount();
                if (DAL.ProjectVikingsContext.alivePlayers.Count > 0)
                {
                    if (target == null)
                        target = utils.NearTargetInView(DAL.ProjectVikingsContext.alivePlayers.Select(x => x.transform).ToList(), fow.visibleTargets, _transform);
                    else
                    {
                        target = utils.NearTarget(DAL.ProjectVikingsContext.alivePlayers.Select(x => x.transform).ToList(), _transform, target);
                        if (target == null) return;
                        fow.TurnView(target);
                    }
                }
            }
        }

        public override int GetDamage()
        {
            var player = enemyFunctions.GetModelById(id);
            return rnd.Next(player.AttackMin.Value, player.AttackMax.Value);
        }

        public void Attack(Transform transform, Vector3 size, LayerMask targetLayer)
        {
            var hitColliders = Physics2D.OverlapBoxAll(PositionCenterAttack(size, transform), size, 90f, targetLayer);
            foreach (var hitCollider in hitColliders)
            {
                if (targetsAttacked.Contains(hitCollider.gameObject.GetInstanceID())) continue;
                targetsAttacked.Add(hitCollider.gameObject.GetInstanceID());

                var script = hitCollider.gameObject.GetComponent<MonoBehaviour>();
                SystemManagement.SystemManagement.CallMethod(script, "GetDamage", new object[] { GetDamage(), transform.position, true });
            }
        }

        public override Vector3 PositionCenterAttack(Vector3 colSize, Transform transform)
        {
            if (target == null) return new Vector3();
                return transform.position + PositionAttack(colSize, GetDirection(transform.position, target.position));
        }

        public void SetFieldOfView(FieldOfView fow)
        {
            this.fow = fow;
        }

        public KeyMove GetInput(PossibleMoviment lastMoviment)
        {
            switch (lastMoviment)
            {
                case PossibleMoviment.Down:
                    return new KeyMove(null, new Vector2(0, -1), false);
                case PossibleMoviment.Down_Left:
                    return new KeyMove(null, new Vector2(-1, -1), false);
                case PossibleMoviment.Down_Right:
                    return new KeyMove(null, new Vector2(1, -1), true);
                case PossibleMoviment.Left:
                    return new KeyMove(null, new Vector2(-1, 0), false);
                case PossibleMoviment.Right:
                    return new KeyMove(null, new Vector2(1, 0), true);
                case PossibleMoviment.Up:
                    return new KeyMove(null, new Vector2(0, 1), false);
                case PossibleMoviment.Up_Left:
                    return new KeyMove(null, new Vector2(-1, 1), false);
                case PossibleMoviment.Up_Right:
                    return new KeyMove(null, new Vector2(1, 1), true);
                default:
                    return new KeyMove(null, new Vector2(0, 0), false);
            }
        }
        public Models.EnemyViewModel GetInitialData(GameObject go)
        {
            var data = enemyFunctions.GetDataByInitialPosition(go.transform.position);
            if (data == null)
            {
                data = DAL.ProjectVikingsContext.defaultEnemy;
                data.InitialX = go.transform.position.x;
                data.InitialY = go.transform.position.y;
                enemyFunctions.Create(data);
            }
            id = data.EnemyId;
            var model = enemyFunctions.GetDataViewModel(data);
            model.GameObject = go;
            enemyFunctions.SetModel(model);

            return model;
        }
    }
}
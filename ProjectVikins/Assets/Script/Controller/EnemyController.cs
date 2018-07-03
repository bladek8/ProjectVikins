using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Script.Helpers;
using Assets.Script.Models;
using Assets.Script.BLL;

namespace Assets.Script.Controller
{
    public class EnemyController : Shared._CharacterController<Models.EnemyViewModel>
    {
        private readonly BLL.EnemyFunctions enemyFunctions = new BLL.EnemyFunctions();
        private int id;
        List<Transform> players;
        private Utils utils = new Utils();
        public bool canAttack;
        public FieldOfView fow;

        public CountDown followPlayer = new CountDown();
        public Transform target;

        public EnemyController()
        {
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
                _transform.position = Vector3.MoveTowards(_transform.position, target.transform.position, enemyFunctions.GetModelById(id).SpeedWalk.Value * Time.deltaTime);
                fow.TurnView(target);
                enemyFunctions.UpdateStats(new Models.EnemyViewModel() { LastMoviment = GetDirection(_transform), EnemyId = id });
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
            var player = enemyFunctions.GetModelById(id);
            return rnd.Next(player.AttackMin.Value, player.AttackMax.Value);
        }

        public override void UpdateStats(EnemyViewModel model)
        {
            throw new NotImplementedException();
        }

        public override void Decrease(EnemyViewModel model)
        {
            throw new NotImplementedException();
        }

        public override void Increase(EnemyViewModel model)
        {
            throw new NotImplementedException();
        }

        public void Attack(Transform transform, Vector3 size, LayerMask targetLayer)
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
            var player = enemyFunctions.GetModelById(id);
            return transform.position + PositionAttack(colSize, GetDirection(transform));
        }

        public void SetFieldOfView(FieldOfView fow)
        {
            this.fow = fow;
        }

        public Helpers.KeyMove GetInput()
        {
            var enemy = enemyFunctions.GetModelById(id);

            switch (enemy.LastMoviment)
            {
                case Helpers.PossibleMoviment.Down:
                    return new Helpers.KeyMove(null,new Vector2(0, -1),false);
                case Helpers.PossibleMoviment.Down_Left:
                    return new Helpers.KeyMove(null,new Vector2(-1, -1),false);
                case Helpers.PossibleMoviment.Down_Right:
                    return new Helpers.KeyMove(null, new Vector2(1, -1), true);
                case Helpers.PossibleMoviment.Left:
                    return new Helpers.KeyMove(null, new Vector2(-1, 0), false);
                case Helpers.PossibleMoviment.Right:
                    return new Helpers.KeyMove(null, new Vector2(1, 0), true);
                case Helpers.PossibleMoviment.Up:
                    return new Helpers.KeyMove(null, new Vector2(0, 1), false);
                case Helpers.PossibleMoviment.Up_Left:
                    return new Helpers.KeyMove(null, new Vector2(-1, 1), false);
                case Helpers.PossibleMoviment.Up_Right:
                    return new Helpers.KeyMove(null, new Vector2(1, 1), true);
                default:
                    return new Helpers.KeyMove(null, new Vector2(0, 0), false);
            }
        }
        public DAL.Enemy GetInitialData(Vector3 position)
        {
            var data = enemyFunctions.GetDataByInitialPosition(position);
            if (data == null)
            {
                data = DAL.MVC_Game2Context.defaultEnemy;
                data.InitialX = position.x;
                data.InitialY = position.y;
                enemyFunctions.Create(data);
            }
            id = data.EnemyId;
            var model = enemyFunctions.GetDataViewModel(data);
            enemyFunctions.SetModel(model);

            return data;
        }
    }
}
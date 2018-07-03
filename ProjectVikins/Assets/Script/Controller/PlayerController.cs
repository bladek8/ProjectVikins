﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Models;
using UnityEngine;
using Assets.Script.Helpers;
using System.IO;
using Assets.Script.BLL;

namespace Assets.Script.Controller
{
    public class PlayerController : Shared._CharacterController<Models.PlayerViewModel>
    {
        private Helpers.Utils utils = new Helpers.Utils();
        private readonly BLL.PlayerFunctions playerFunctions = new BLL.PlayerFunctions();
        private int id;
        private readonly GameObject gameObj;
        public FieldOfView fow;
        public bool canAttack;
        public CountDown followEnemy = new CountDown();
        public Transform target;
        List<Transform> enemies;

        public PlayerController(GameObject gameObj)
        {
            this.gameObj = gameObj;
            enemies = utils.GetTransformInLayer("Enemy");
        }

        public void Attack(Transform transform, Vector3 size)
        {
            var hitColliders = Physics2D.OverlapBoxAll(PositionCenterAttack(size, transform), size, 90f);
            foreach (var hitCollider in hitColliders)
            {

                if (targetsAttacked.Contains(hitCollider.gameObject.GetInstanceID())) continue;
                targetsAttacked.Add(hitCollider.gameObject.GetInstanceID());

                if (hitCollider.tag == "Enemy")
                {
                    var currentLife = hitCollider.gameObject.GetComponent<View.EnemyView>().model.Life -= GetDamage();
                    if (currentLife <= 0)
                        Destroy(hitCollider.gameObject);
                }
                if (hitCollider.tag == "NPC")
                {
                    var NPCView = hitCollider.GetComponent(typeof(Component));
                    NPCInteraction(NPCView);
                }
            }
        }

        public override Vector3 PositionCenterAttack(Vector3 colSize, Transform transform)
        {
            var player = playerFunctions.GetModelById(id);
            return transform.position + PositionAttack(colSize, player.LastMoviment.Value);
        }

        public void SetLastMoviment(float inputX, float inputY)
        {
            var player = playerFunctions.GetModelById(id);

            if (inputX > 0 && inputY > 0)
                player.LastMoviment = Helpers.PossibleMoviment.Up_Right;
            else if (inputX < 0 && inputY > 0)
                player.LastMoviment = Helpers.PossibleMoviment.Up_Left;
            else if (inputX == 0 && inputY > 0)
                player.LastMoviment = Helpers.PossibleMoviment.Up;
            else if (inputX > 0 && inputY == 0)
                player.LastMoviment = Helpers.PossibleMoviment.Right;
            else if (inputX < 0 && inputY == 0)
                player.LastMoviment = Helpers.PossibleMoviment.Left;
            else if (inputX < 0 && inputY < 0)
                player.LastMoviment = Helpers.PossibleMoviment.Down_Left;
            else if (inputX > 0 && inputY < 0)
                player.LastMoviment = Helpers.PossibleMoviment.Down_Right;
            else if (inputX == 0 && inputY < 0)
                player.LastMoviment = Helpers.PossibleMoviment.Down;
            else
                player.LastMoviment = Helpers.PossibleMoviment.None;
        }

        public void Walk(Vector2 vector)
        {
            var player = playerFunctions.GetModelById(id);
            gameObj.transform.Translate(vector * Time.deltaTime * player.SpeedWalk);
        }

        public override int GetDamage()
        {
            var player = playerFunctions.GetModelById(id);
            return rnd.Next(player.AttackMin, player.AttackMax);
        }

        //public override void UpdateStats(PlayerViewModel model)
        //{
        //    model.PlayerId = id;
        //    playerFunctions.UpdateStats(model);
        //}

        //public override void Decrease(PlayerViewModel model)
        //{
        //    model.PlayerId = id;
        //    playerFunctions.Decrease(model);
        //}

        //public override void Increase(PlayerViewModel model)
        //{
        //    model.PlayerId = id;
        //    playerFunctions.Increase(model);
        //}

        public void WalkToPlayer(Transform _transform, Transform controllablePlayer, ref PlayerViewModel model)
        {
            target = controllablePlayer.transform;
            if (Math.Abs(Vector3.Distance(target.transform.position, _transform.position)) > 0.5)
            {
                _transform.position = Vector3.MoveTowards(_transform.position, target.transform.position, playerFunctions.GetModelById(id).SpeedWalk * Time.deltaTime);
                fow.TurnView(target);
                model.LastMoviment = GetDirection(_transform, target);
            }
        }


        public void WalkTowardTo(Transform _transform, ref PlayerViewModel model)
        {
            if (target != null && followEnemy.CoolDown <= 0 || target == null)
            {
                followEnemy.StartToCount();
                if (target == null)
                    target = utils.NearTargetInView(enemies, fow.visibleTargets, _transform);
                else
                {
                    target = utils.NearTarget(enemies, _transform, target);
                    fow.TurnView(target);
                }
            }
            else if (fow.visibleTargets.Contains(target))
            {
                if (target == null) return;
                if (Math.Abs(Vector3.Distance(target.transform.position, _transform.position)) > 0.5)
                {
                    _transform.position = Vector3.MoveTowards(_transform.position, target.transform.position, playerFunctions.GetModelById(id).SpeedWalk * Time.deltaTime);
                    fow.TurnView(target);
                    model.LastMoviment = GetDirection(_transform, target);
                    //playerFunctions.UpdateStats(new Models.PlayerViewModel() { LastMoviment = GetDirection(_transform, target), PlayerId = id });
                    canAttack = false;
                }
                else
                    canAttack = true;
            }
            else
            {
                canAttack = false;
                target = null;
            }
        }

        public void ChangeControllableCharacter()
        {
            playerFunctions.ChangeControllableCharacter(id);
        }

        public bool GetIsControllable()
        {
            return playerFunctions.GetModelById(id).IsBeingControllable;
        }

        public bool GetPlayerMode()
        {
            return playerFunctions.GetModelById(id).IsBeingControllable;
        }

        public void SetFieldOfView(FieldOfView fow)
        {
            this.fow = fow;
        }

        public Helpers.KeyMove GetInput()
        {
            var player = playerFunctions.GetModelById(id);

            switch (player.LastMoviment)
            {
                case Helpers.PossibleMoviment.Down:
                    return new Helpers.KeyMove(null, new Vector2(0, -1), false);
                case Helpers.PossibleMoviment.Down_Left:
                    return new Helpers.KeyMove(null, new Vector2(-1, -1), false);
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


        //public Helpers.PossibleMoviment GetDirection(Transform transform)
        //{
        //    var vectorDirection = target.position - transform.position;
        //    var degrees = Mathf.Atan2(vectorDirection.y, vectorDirection.x) * Mathf.Rad2Deg;
        //    var position = (int)((Mathf.Round(degrees / 45f) + 8) % 8);

        //    switch (position)
        //    {
        //        case 0:
        //            return Helpers.PossibleMoviment.Right;
        //        case 1:
        //            return Helpers.PossibleMoviment.Up_Right;
        //        case 2:
        //            return Helpers.PossibleMoviment.Up;
        //        case 3:
        //            return Helpers.PossibleMoviment.Up_Left;
        //        case 4:
        //            return Helpers.PossibleMoviment.Left;
        //        case 5:
        //            return Helpers.PossibleMoviment.Down_Left;
        //        case 6:
        //            return Helpers.PossibleMoviment.Down;
        //        case 7:
        //            return Helpers.PossibleMoviment.Down_Right;
        //        default:
        //            return Helpers.PossibleMoviment.None;
        //    }
        //}

        public Models.PlayerViewModel GetInitialData(Transform transform)
        {
            var data = playerFunctions.GetDataByInitialPosition(transform.position);
            if (data == null)
            {
                data = DAL.MVC_Game2Context.defaultPlayer;
                data.InitialX = transform.position.x;
                data.InitialY = transform.position.y;
                playerFunctions.Create(data);
            }
            id = data.PlayerId;
            var model = playerFunctions.GetDataViewModel(data);
            model.transform = transform;
            playerFunctions.SetModel(model);
            return model;
        }

        public void SaveData()
        {
            BLL.SaveFunctions.Save();
        }
    }
}

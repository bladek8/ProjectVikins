using System;
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
        public FieldOfView fow;
        public bool canAttack;
        public CountDown followEnemy = new CountDown();
        public Transform target = null;
        List<Models.EnemyViewModel> enemies;

        public PlayerController()
        {
            enemies = DAL.MVC_Game2Context.enemieModels;
        }
        
        public void Attack(Transform transform, Vector3 size)
        {
            playerFunctions.AttackMode();
            var hitColliders = Physics2D.OverlapBoxAll(PositionCenterAttack(size, transform), size, 90f);
            foreach (var hitCollider in hitColliders)
            {
                if (targetsAttacked.Contains(hitCollider.gameObject.GetInstanceID())) continue;
                targetsAttacked.Add(hitCollider.gameObject.GetInstanceID());

                if (hitCollider.tag == "Enemy")
                {
                    var script = hitCollider.gameObject.GetComponent<MonoBehaviour>();
                    script.SendMessage("GetDamage", GetDamage());
                }
                else if (hitCollider.tag == "NPC")
                {
                    var NPCView = hitCollider.GetComponent(typeof(Component));
                    NPCInteraction(NPCView);
                }
                else if (hitCollider.tag == "Bonfire")
                {
                    SaveData();
                    Debug.Log("Save!");
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

        public void Walk(Vector2 vector, float speed)
        {
            playerFunctions.GetModelById(id).GameObject.transform.Translate(vector * Time.deltaTime * speed);
        }

        public override int GetDamage()
        {
            var player = playerFunctions.GetModelById(id);
            return rnd.Next(player.AttackMin, player.AttackMax);
        }

        public void WalkToPlayer(Transform _transform, Transform controllablePlayer, ref PlayerViewModel model)
        {
            var target = controllablePlayer.transform;
            if (Math.Abs(Vector3.Distance(target.transform.position, _transform.position)) > 0.5)
            {
                _transform.position = Vector3.MoveTowards(_transform.position, target.transform.position, playerFunctions.GetModelById(id).SpeedWalk * Time.deltaTime);
                fow.TurnView(target);
                model.LastMoviment = GetDirection(_transform.position, target.position);
            }
        }


        public void WalkTowardTo(Transform _transform, ref PlayerViewModel model)
        {
            if (fow.visibleTargets.Contains(target))
            {
                if (target == null) return;
                _transform.position = Vector3.MoveTowards(_transform.position, target.transform.position, playerFunctions.GetModelById(id).SpeedWalk * Time.deltaTime);
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
            if (target != null && followEnemy.CoolDown <= 0 || target == null)
            {
                followEnemy.StartToCount();
                if (enemies.Count > 0)
                {
                    if (target == null)
                        target = utils.NearTargetInView(enemies.Select(x => x.GameObject.transform).ToList(), fow.visibleTargets, _transform);
                    else
                    {
                        target = utils.NearTarget(enemies.Select(x => x.GameObject.transform).ToList(), _transform, target);
                        fow.TurnView(target);
                    }
                }
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

        public Models.PlayerViewModel GetInitialData(GameObject go)
        {
            var data = playerFunctions.GetDataByInitialPosition(go.transform.position);
            if (data == null)
            {
                data = DAL.MVC_Game2Context.defaultPlayer;
                data.InitialX = go.transform.position.x;
                data.InitialY = go.transform.position.y;
                playerFunctions.Create(data);
            }
            id = data.PlayerId;
            var model = playerFunctions.GetDataViewModel(data);
            model.GameObject = go;
            playerFunctions.SetModel(model);
            return model;
        }

        public void SaveData()
        {
            BLL.SaveFunctions.Save();
        }

        public void SetForceToWalk(bool value)
        {
            foreach(var playerModel in playerFunctions.GetModels())
            {
                playerModel.ForceToWalk = value;
            }
        }

        public void SetForceToStop(bool value)
        {
            foreach (var playerModel in playerFunctions.GetModels())
            {
                playerModel.ForceToStop = value;
            }
        }
    }
}

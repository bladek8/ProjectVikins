using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Models;
using Assets.Script.View;
using UnityEngine;
using Assets.Script.Helpers;
using System.IO;
using Assets.Script.BLL;
using Assets.Script.SystemManagement;
using Assets.Script.DAL;

namespace Assets.Script.Controller
{
    public class PlayerController : Shared._CharacterController<Models.PlayerViewModel>
    {
        private Helpers.Utils utils = new Helpers.Utils();

        private readonly BLL.PlayerFunctions playerFunctions = new BLL.PlayerFunctions();
        private readonly BLL.EnemyFunctions enemyFunctions = new BLL.EnemyFunctions();

        private readonly InventoryItemFunctions inventoryItemFunctions = new InventoryItemFunctions();

        private int id;
        public FieldOfView fow;
        public bool canAttack;
        public CountDown followEnemy = new CountDown();
        public Transform target = null;
        List<Models.EnemyViewModel> enemies;

        public PlayerController()
        {
        }

        public void Attack(Transform transform, Vector3 size)
        {
            var hitColliders = Physics2D.OverlapBoxAll(PositionCenterAttack(size, transform), size, 90f);
            foreach (var hitCollider in hitColliders)
            {
                if (targetsAttacked.Contains(hitCollider.gameObject.GetInstanceID())) continue;
                targetsAttacked.Add(hitCollider.gameObject.GetInstanceID());

                if (hitCollider.transform == transform) continue;

                if (hitCollider.tag == "Enemy")
                {
                    target = hitCollider.transform;
                    var script = hitCollider.gameObject.GetComponent<MonoBehaviour>();
                    var killed = script.CallMethod("GetDamage", new object[] { GetDamage(), transform.position, true });
                    if (killed != null)
                    {
                        if ((bool)killed)
                        {
                            target = null;
                            if (!playerFunctions.GetDataById(id).IsBeingControllable)
                                FindTarget(transform);
                        }
                    }
                }
            }
        }

        public void Interact(Transform transform, Vector3 size)
        {
            var hitColliders = Physics2D.OverlapBoxAll(PositionCenterAttack(size, transform), size, 90f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.transform == transform) continue;

                if (hitCollider.tag == "NPC" && View.NPCView.canInteract)
                {
                    View.NPCView.canInteract = false;
                    var NPCView = hitCollider.GetComponent(typeof(Component));
                    NPCInteraction(NPCView);
                }
                else if (hitCollider.tag == "Bonfire")
                {
                    SaveData();
                    Debug.Log("Save!");
                }
                else if (hitCollider.tag == "Player")
                {
                    var script = hitCollider.gameObject.GetComponent<MonoBehaviour>();
                    script.SendMessage("StartSavePlayer");
                }
                else if (hitCollider.tag == "Collectable")
                {
                    var script = hitCollider.gameObject.GetComponent<MonoBehaviour>();
                    var itemModel = script.CallMethod("GetItemModel");
                    if (itemModel != null)
                        inventoryItemFunctions.Create(itemModel);

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
                if (DAL.ProjectVikingsContext.aliveEnemies.Count > 0)
                {
                    if (target == null)
                        target = utils.NearTargetInView(DAL.ProjectVikingsContext.aliveEnemies.Select(x => x.transform).ToList(), fow.visibleTargets, _transform);
                    else
                    {
                        target = utils.NearTarget(DAL.ProjectVikingsContext.aliveEnemies.Select(x => x.transform).ToList(), _transform, target);
                        if (target == null) return;
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

        public Helpers.KeyMove GetInput(PossibleMoviment lastMoviment)
        {
            switch (lastMoviment)
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
                data = DAL.ProjectVikingsContext.defaultPlayer;
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
            foreach (var playerModel in playerFunctions.GetModels())
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

        public void AttackMode()
        {
            playerFunctions.AttackMode();
        }

        public static void GetSliderEnemy(Transform target)
        {
            var enemy = ProjectVikingsContext.enemieModels.Where(x => x.GameObject.transform == target).ToList();
            if (enemy.Count() == 0)
                return;
            else
                SliderView.model = enemy.First();
        }

        public void Defend(ref PlayerViewModel model)
        {
            int currentDirection = (int)model.LastMoviment;
            if (currentDirection == 0) model.DirectionsDefended = null;
            var directionsDefended = new List<int> { currentDirection };
            if (currentDirection + 1 == 9)
                directionsDefended.Add(1);
            else
                directionsDefended.Add(currentDirection + 1);
            if (currentDirection - 1 == 0)
                directionsDefended.Add(8);
            else
                directionsDefended.Add(currentDirection - 1);

            model.DirectionsDefended = directionsDefended;
        }

    }
}

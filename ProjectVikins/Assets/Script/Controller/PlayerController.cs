﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Models;
using UnityEngine;

namespace Assets.Script.Controller
{
    public class PlayerController : Shared._CharacterController<Models.PlayerViewModel>
    {
        private Helpers.Utils utils = new Helpers.Utils();
        private readonly BLL.PlayerFunctions playerFunctions;
        private readonly int id;
        private readonly GameObject gameObj;

        public PlayerController(Models.PlayerViewModel model, GameObject gameObj)
        {
            playerFunctions = new BLL.PlayerFunctions();
            playerFunctions.Create(model);
            this.id = model.PlayerId;
            this.gameObj = gameObj;
        }

        public override void Attack(Transform transform, Vector3 size, LayerMask targetLayer)
        {
            var hitColliders = Physics2D.OverlapBoxAll(PositionCenterAttack(size, transform), size, 90f, targetLayer);
            foreach (var hitCollider in hitColliders)
            {

                if (targetsAttacked.Contains(hitCollider.gameObject.GetInstanceID()))   continue;
                targetsAttacked.Add(hitCollider.gameObject.GetInstanceID());
                
                if (Convert.ToInt32(DecreaseStats(hitCollider.gameObject.name, "Life", GetDamage(), hitCollider.gameObject.GetInstanceID())) <= 0)
                    Destroy(hitCollider.gameObject);
            }
        }

        public override Vector3 PositionCenterAttack(Vector3 colSize, Transform transform)
        {
            var player = playerFunctions.GetDataById(id);
            return transform.position + PositionAttack(colSize, player.LastMoviment);
        }

        public void SetLastMoviment(float inputX, float inputY)
        {
            var player = playerFunctions.GetDataById(id);

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
            var player = playerFunctions.GetDataById(id);
            gameObj.transform.Translate(vector * Time.deltaTime * player.SpeedWalk);
        }

        public override int GetDamage()
        {
            var player = playerFunctions.GetDataById(id);
            return rnd.Next(player.AttackMin, player.AttackMax);
        }

        public override void UpdateStats(PlayerViewModel model)
        {
            model.PlayerId = id;
            playerFunctions.UpdateStats(model);
        }

        public override void Decrease(PlayerViewModel model)
        {
            model.PlayerId = id;
            playerFunctions.Decrease(model);
        }

        public override void Increase(PlayerViewModel model)
        {
            model.PlayerId = id;
            playerFunctions.Increase(model);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Models;
using UnityEngine;

namespace Assets.Script.Controller
{
    public class PlayerController : Shared.CharacterController<Models.PlayerViewModel>
    {
        System.Random rnd = new System.Random();
        private Helpers.Utils utils = new Helpers.Utils();
        private readonly BLL.PlayerFunctions playerFunctions;
        private readonly int id;
        private readonly GameObject gameObj;

        public PlayerController(DAL.Player model, GameObject gameObj)
            : base("PlayerFunctions")
        {
            playerFunctions = new BLL.PlayerFunctions();
            playerFunctions.Create(model);
            this.id = model.PlayerId;
            this.gameObj = gameObj;
        }

        public Vector2 Attack(Vector2 colSize)
        {
            var player = playerFunctions.GetDataById(id);
            if (player.LastMoviment == Helpers.PossibleMoviment.Down)
                return new Vector2(0, -(colSize.y/2));
            if (player.LastMoviment == Helpers.PossibleMoviment.Down_Left)
                return new Vector2(-(colSize.x / 2), -(colSize.y / 2));
            if (player.LastMoviment == Helpers.PossibleMoviment.Down_Right)
                return new Vector2(colSize.x / 2, -(colSize.y / 2));
            if (player.LastMoviment == Helpers.PossibleMoviment.Left)
                return new Vector2(-colSize.x, 0);
            if (player.LastMoviment == Helpers.PossibleMoviment.Right)
                return new Vector2(colSize.x, 0);
            if (player.LastMoviment == Helpers.PossibleMoviment.Up)
                return new Vector2(0, colSize.y / 2);
            if (player.LastMoviment == Helpers.PossibleMoviment.Up_Left)
                return new Vector2(-(colSize.x / 2), colSize.y / 2);
            if (player.LastMoviment == Helpers.PossibleMoviment.Up_Right)
                return new Vector2(colSize.x / 2, colSize.y / 2);

            return new Vector2(0, 0);
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
            return rnd.Next(player.AttackMin , player.AttackMax);
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

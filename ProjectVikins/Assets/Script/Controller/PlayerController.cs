using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Controller
{
    public class PlayerController : Shared.CharacterController<BLL.PlayerFunctions>
    {
        public Helpers.Utils utils = new Helpers.Utils();
        private readonly BLL.PlayerFunctions playerFunctions;
        private readonly int id;
        private readonly GameObject gameObj;

        public PlayerController(DAL.Player model, GameObject gameObj)
        {
            playerFunctions = new BLL.PlayerFunctions(model);
            this.id = model.PlayerId;
            this.gameObj = gameObj;
            SetFunction();
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

        public override void SetFunction()
        {
            this._TFuncitons = playerFunctions;
        }
    }
}

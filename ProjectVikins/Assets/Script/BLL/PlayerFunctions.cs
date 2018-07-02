using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.DAL;
using Assets.Script.Models;

namespace Assets.Script.BLL
{
    public class PlayerFunctions : Shared.BLLFunctions<DAL.Player, Models.PlayerViewModel>
    {
        public PlayerFunctions()
            : base("PlayerId")
        {
            SetListContext();
        }
        public override int Create(Models.PlayerViewModel model)
        {
            var player = new DAL.Player()
            {
                AttackMax = model.AttackMax.Value,
                CharacterTypeId = model.CharacterTypeId.Value,
                AttackMin = model.AttackMin.Value,
                PlayerId = model.PlayerId,
                Life = model.Life.Value,
                SpeedRun = model.SpeedRun.Value,
                SpeedWalk = model.SpeedWalk.Value,
                IsBeingControllable = model.IsBeingControllable.Value,
                PlayerMode = model.PlayerMode.Value,
                InitialX = model.InitialX.Value,
                InitialY = model.InitialY.Value,
                LastMoviment = model.LastMoviment.Value
            };
            ListContext.Add(player);
            return player.PlayerId;
        }

        public int Create(Player data)
        {
            ListContext.Add(data);
            return data.PlayerId;
        }

        public PlayerViewModel GetDataViewModel(Player data)
        {
            return new PlayerViewModel()
            {
                AttackMax = data.AttackMax,
                AttackMin = data.AttackMin,
                CharacterTypeId = data.CharacterTypeId,
                InitialX = data.InitialX,
                InitialY = data.InitialY,
                IsBeingControllable = data.IsBeingControllable,
                LastMoviment = data.LastMoviment,
                Life = data.Life,
                PlayerId = data.PlayerId,
                PlayerMode = data.PlayerMode,
                SpeedRun = data.SpeedRun,
                SpeedWalk = data.SpeedWalk
            };
        }

        public override void SetListContext()
        {
            this.ListContext = DAL.MVC_Game2Context.players;
        }

        public override void UpdateStats(PlayerViewModel model)
        {
            var player = this.GetDataById(model.PlayerId);

            if (model.LastMoviment.HasValue) player.LastMoviment = model.LastMoviment.Value;
            if (model.Life.HasValue) player.Life = model.Life.Value;
            if (model.SpeedRun.HasValue) player.SpeedRun = model.SpeedRun.Value;
            if (model.SpeedWalk.HasValue) player.SpeedWalk = model.SpeedWalk.Value;
            if (model.AttackMin.HasValue) player.AttackMin = model.AttackMin.Value;
            if (model.AttackMax.HasValue) player.AttackMax = model.AttackMax.Value;
            if (model.IsBeingControllable.HasValue) player.IsBeingControllable = model.IsBeingControllable.Value;
            if (model.PlayerMode.HasValue) player.PlayerMode = model.PlayerMode.Value;
            if (model.Transform != null) player.Transform = model.Transform;
        }

        public override void Decrease(PlayerViewModel model)
        {
            var player = this.GetDataById(model.PlayerId);

            if (model.Life.HasValue) player.Life = player.Life - model.Life.Value;
            if (model.SpeedRun.HasValue) player.SpeedRun = player.SpeedRun - model.SpeedRun.Value;
            if (model.SpeedWalk.HasValue) player.SpeedWalk = player.SpeedWalk - model.SpeedWalk.Value;
            if (model.AttackMin.HasValue) player.AttackMin = player.AttackMin - model.AttackMin.Value;
            if (model.AttackMax.HasValue) player.AttackMax = player.AttackMax - model.AttackMax.Value;
        }

        public override void Increase(PlayerViewModel model)
        {
            var player = this.GetDataById(model.PlayerId);

            if (model.Life.HasValue) player.Life = player.Life + model.Life.Value;
            if (model.SpeedRun.HasValue) player.SpeedRun = player.SpeedRun + model.SpeedRun.Value;
            if (model.SpeedWalk.HasValue) player.SpeedWalk = player.SpeedWalk + model.SpeedWalk.Value;
            if (model.AttackMin.HasValue) player.AttackMin = player.AttackMin + model.AttackMin.Value;
            if (model.AttackMax.HasValue) player.AttackMax = player.AttackMax + model.AttackMax.Value;
        }

        public void ChangeControllableCharacter(int id)
        {
            var player = this.GetDataById(id);
            player.IsBeingControllable = false;
            var nextPlayer = new DAL.Player();
            try
            {
                nextPlayer = ListContext[ListContext.IndexOf(player) + 1];
            }
            catch (Exception ex)
            {
                nextPlayer = ListContext.First();
            }
            nextPlayer.IsBeingControllable = true;
        }
    }
}

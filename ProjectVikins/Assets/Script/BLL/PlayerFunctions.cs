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
            SetListModel();
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

        public override int SetModel(Models.PlayerViewModel model)
        {
            ListModel.Add(model);
            return model.PlayerId;
        }

        public int Create(Player data)
        {
            ListContext.Add(data);
            return data.PlayerId;
        }

        public override PlayerViewModel GetDataViewModel(Player data)
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

        public override List<PlayerViewModel> GetDataViewModel(List<Player> data)
        {
            var r = (from y in data.ToList()
                     select new PlayerViewModel()
                     {
                         AttackMax = y.AttackMax,
                         AttackMin = y.AttackMin,
                         CharacterTypeId = y.CharacterTypeId,
                         InitialX = y.InitialX,
                         InitialY = y.InitialY,
                         IsBeingControllable = y.IsBeingControllable,
                         LastMoviment = y.LastMoviment,
                         Life = y.Life,
                         PlayerId = y.PlayerId,
                         PlayerMode = y.PlayerMode,
                         SpeedRun = y.SpeedRun,
                         SpeedWalk = y.SpeedWalk
                     }).ToList();
            return r;
        }

        public override void SetListContext()
        {
            this.ListContext = DAL.MVC_Game2Context.players;
        }

        public override void SetListModel()
        {
            this.ListModel = DAL.MVC_Game2Context.playerModels;
        }

        public override void UpdateStats(PlayerViewModel model)
        {
            var player = this.GetModelById(model.PlayerId);

            if (model.LastMoviment.HasValue) player.LastMoviment = model.LastMoviment.Value;
            if (model.Life.HasValue) player.Life = model.Life.Value;
            if (model.SpeedRun.HasValue) player.SpeedRun = model.SpeedRun.Value;
            if (model.SpeedWalk.HasValue) player.SpeedWalk = model.SpeedWalk.Value;
            if (model.AttackMin.HasValue) player.AttackMin = model.AttackMin.Value;
            if (model.AttackMax.HasValue) player.AttackMax = model.AttackMax.Value;
            if (model.IsBeingControllable.HasValue) player.IsBeingControllable = model.IsBeingControllable.Value;
            if (model.PlayerMode.HasValue) player.PlayerMode = model.PlayerMode.Value;
            if (model.transform != null) { player.transform = model.transform; }
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
            var player = this.GetModelById(id);
            player.IsBeingControllable = false;
            var nextPlayer = new Models.PlayerViewModel();
            var index = ListModel.IndexOf(player) + 1;
            if (ListModel.Count > index)
                nextPlayer = ListModel[index];
            else
                nextPlayer = ListModel.First();
            nextPlayer.IsBeingControllable = true;
        }

        public override Player GetDataByViewModel(PlayerViewModel model)
        {
            var player = GetDataById(model.PlayerId);

            player.AttackMax = model.AttackMax.Value;
            player.AttackMin = model.AttackMin.Value;
            player.CharacterTypeId = player.CharacterTypeId;
            player.InitialX = player.InitialX;
            player.InitialY = player.InitialY;
            player.IsBeingControllable = model.IsBeingControllable.Value;
            player.LastMoviment = model.LastMoviment.Value;
            player.Life = model.Life.Value;
            player.PlayerId = player.PlayerId;
            player.PlayerMode = model.PlayerMode.Value;
            player.SpeedRun = model.SpeedRun.Value;
            player.SpeedWalk = model.SpeedWalk.Value;
            player.X = model.transform.position.x;
            player.Y = model.transform.position.y;

            return player;
        }

        public override List<Player> GetDataByViewModel(List<PlayerViewModel> model)
        {
            return (from y in model
                     select new Player() {
                         AttackMax = y.AttackMax.Value,
                         AttackMin = y.AttackMin.Value,
                         CharacterTypeId = y.CharacterTypeId.Value,
                         InitialX = y.InitialX.Value,
                         InitialY = y.InitialY.Value,
                         IsBeingControllable = y.IsBeingControllable.Value,
                         LastMoviment = y.LastMoviment.Value,
                         Life = y.Life.Value,
                         PlayerId = y.PlayerId,
                         PlayerMode = y.PlayerMode.Value,
                         SpeedRun = y.SpeedRun.Value,
                         SpeedWalk = y.SpeedWalk.Value,
                         X = y.transform.position.x,
                         Y = y.transform.position.y,
                     }).ToList();
        }
    }
}

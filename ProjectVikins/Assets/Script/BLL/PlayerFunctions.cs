using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.DAL;
using Assets.Script.Helpers;
using Assets.Script.Models;

namespace Assets.Script.BLL
{
    public class PlayerFunctions : Shared.BLLFunctions<Player, PlayerViewModel>
    {
        public PlayerFunctions()
            : base("PlayerId")
        {
        }

        public override int Create(Models.PlayerViewModel model)
        {
            var player = new DAL.Player()
            {
                AttackMax = model.AttackMax,
                CharacterTypeId = model.CharacterTypeId.Value,
                AttackMin = model.AttackMin,
                PlayerId = model.PlayerId,
                CurrentLife = model.CurrentLife,
                MaxLife = model.MaxLife,
                SpeedRun = model.SpeedRun,
                SpeedWalk = model.SpeedWalk,
                IsBeingControllable = model.IsBeingControllable,
                PlayerMode = model.PlayerMode.Value,
                InitialX = model.InitialX,
                InitialY = model.InitialY,
                LastMoviment = (int)model.LastMoviment.Value,
                IsTank = model.IsTank
            };
            ListContext.Add(player);
            return player.PlayerId;
        }

        public override int SetModel(Models.PlayerViewModel model)
        {
            model.InternalPlayerId = ListModel.Count + 1;

            ListModel.Add(model);
            ProjectVikingsContext.UpdateAliveLists();
            return model.InternalPlayerId;
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
                LastMoviment = (PossibleMoviment)data.LastMoviment,
                CurrentLife = data.CurrentLife,
                MaxLife = data.MaxLife,
                PlayerId = data.PlayerId,
                PlayerMode = data.PlayerMode,
                SpeedRun = data.SpeedRun,
                SpeedWalk = data.SpeedWalk,
                IsTank = data.IsTank
            };
        }

        public override List<PlayerViewModel> GetDataViewModel(IEnumerable<Player> data)
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
                         LastMoviment = (PossibleMoviment)y.LastMoviment,
                         CurrentLife = y.CurrentLife,
                         MaxLife = y.MaxLife,
                         PlayerId = y.PlayerId,
                         PlayerMode = y.PlayerMode,
                         SpeedRun = y.SpeedRun,
                         SpeedWalk = y.SpeedWalk,
                         IsTank = y.IsTank
                     }).ToList();
            return r;
        }

        public override void SetListContext()
        {
            this.ListContext = DAL.ProjectVikingsContext.Player.Data;
        }

        public override void SetListModel()
        {
            this.ListModel = DAL.ProjectVikingsContext.playerModels.Entity;
        }

        public void ChangeControllableCharacter(int id)
        {
            var player = this.GetModelById(id);
            PlayerViewModel nextPlayer = null;
            var index = ListModel.IndexOf(player) + 1;
            for (int i = 0; i < ListModel.Count - 1; i++)
            {
                if (index == ListModel.Count) index = 0;
                if (!ListModel[index].IsDead)
                {
                    nextPlayer = ListModel[index];
                    break;
                }
                index++;
            }

            if (nextPlayer != null)
            {
                nextPlayer.IsBeingControllable = true;
                player.IsBeingControllable = false;
            }
        }

        public override Player GetDataByViewModel(PlayerViewModel model)
        {
            var player = GetDataById(model.PlayerId);

            player.AttackMax = model.AttackMax;
            player.AttackMin = model.AttackMin;
            player.CharacterTypeId = player.CharacterTypeId;
            player.InitialX = player.InitialX;
            player.InitialY = player.InitialY;
            player.IsBeingControllable = model.IsBeingControllable;
            player.LastMoviment = (int)model.LastMoviment.Value;
            player.CurrentLife = model.CurrentLife;
            player.MaxLife = model.MaxLife;
            player.PlayerId = player.PlayerId;
            player.PlayerMode = model.PlayerMode.Value;
            player.SpeedRun = model.SpeedRun;
            player.SpeedWalk = model.SpeedWalk;
            player.X = model.GameObject.transform.position.x;
            player.Y = model.GameObject.transform.position.y;
            player.IsTank = model.IsTank;

            return player;
        }

        public override List<Player> GetDataByViewModel(IEnumerable<PlayerViewModel> model)
        {
            return (from y in model
                    select new Player()
                    {
                        AttackMax = y.AttackMax,
                        AttackMin = y.AttackMin,
                        CharacterTypeId = y.CharacterTypeId.Value,
                        InitialX = y.InitialX,
                        InitialY = y.InitialY,
                        IsBeingControllable = y.IsBeingControllable,
                        LastMoviment = (int)y.LastMoviment.Value,
                        CurrentLife = y.CurrentLife,
                        MaxLife = y.MaxLife,
                        PlayerId = y.PlayerId,
                        PlayerMode = y.PlayerMode.Value,
                        SpeedRun = y.SpeedRun,
                        SpeedWalk = y.SpeedWalk,
                        X = y.GameObject.transform.position.x,
                        Y = y.GameObject.transform.position.y,
                        IsTank = y.IsTank
                    }).ToList();
        }

        public void AttackMode()
        {
            foreach (var y in ListModel)
                y.PlayerMode = Helpers.PlayerModes.Attack;
        }

        public int GetIdOfControllable()
        {
            return this.ListModel.SingleOrDefault(x => x.IsBeingControllable).InternalPlayerId;
        }
    }
}

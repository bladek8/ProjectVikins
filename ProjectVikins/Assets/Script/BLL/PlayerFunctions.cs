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
                LastMoviment = model.LastMoviment.Value
            };
            ListContext.Add(player);
            return player.PlayerId;
        }

        public override int SetModel(Models.PlayerViewModel model)
        {
            ListModel.Add(model);
            ProjectVikingsContext.UpdateAliveLists();
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
                CurrentLife = data.CurrentLife,
                MaxLife = data.MaxLife,
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
                         CurrentLife = y.CurrentLife,
                         MaxLife = y.MaxLife,
                         PlayerId = y.PlayerId,
                         PlayerMode = y.PlayerMode,
                         SpeedRun = y.SpeedRun,
                         SpeedWalk = y.SpeedWalk
                     }).ToList();
            return r;
        }

        public override void SetListContext()
        {
            this.ListContext = DAL.ProjectVikingsContext.players;
        }

        public override void SetListModel()
        {
            this.ListModel = DAL.ProjectVikingsContext.playerModels;
        }

        public void ChangeControllableCharacter(int id)
        {
            var player = this.GetModelById(id);
            PlayerViewModel nextPlayer = null;
            var index = ListModel.IndexOf(player) + 1;
            for (int i = 0; i < ListModel.Count; i++)
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
            player.LastMoviment = model.LastMoviment.Value;
            player.CurrentLife = model.CurrentLife;
            player.MaxLife = model.MaxLife;
            player.PlayerId = player.PlayerId;
            player.PlayerMode = model.PlayerMode.Value;
            player.SpeedRun = model.SpeedRun;
            player.SpeedWalk = model.SpeedWalk;
            player.X = model.GameObject.transform.position.x;
            player.Y = model.GameObject.transform.position.y;

            return player;
        }

        public override List<Player> GetDataByViewModel(List<PlayerViewModel> model)
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
                        LastMoviment = y.LastMoviment.Value,
                        CurrentLife = y.CurrentLife,
                        MaxLife = y.MaxLife,
                        PlayerId = y.PlayerId,
                        PlayerMode = y.PlayerMode.Value,
                        SpeedRun = y.SpeedRun,
                        SpeedWalk = y.SpeedWalk,
                        X = y.GameObject.transform.position.x,
                        Y = y.GameObject.transform.position.y,
                    }).ToList();
        }

        public void AttackMode()
        {
            foreach (var y in ListModel)
                y.PlayerMode = Helpers.PlayerModes.Attack;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.DAL;
using Assets.Script.Models;
using UnityEngine;

namespace Assets.Script.BLL
{
    public class EnemyFunctions : BLL.Shared.BLLFunctions<DAL.Enemy,Models.EnemyViewModel>
    {
        public EnemyFunctions()
            : base("EnemyId")
        {
            SetListContext();
            SetListModel();
        }

        public override int Create(Models.EnemyViewModel model)
        {
            var enemy= new DAL.Enemy()
            {
                AttackMax = model.AttackMax.Value,
                CharacterTypeId = model.CharacterTypeId.Value,
                AttackMin = model.AttackMin.Value,
                EnemyId = model.EnemyId,
                CurrentLife = model.CurrentLife.Value,
                MaxLife = model.MaxLife.Value,
                SpeedRun = model.SpeedRun.Value,
                SpeedWalk = model.SpeedWalk.Value
            };
            ListContext.Add(enemy);
            return enemy.EnemyId;
        }

        public int Create(Enemy data)
        {
            ListContext.Add(data);
            return data.EnemyId;
        }

        public override int SetModel(Models.EnemyViewModel model)
        {
            ListModel.Add(model);
            MVC_Game2Context.UpdateAliveLists();
            return model.EnemyId;
        }
        
        public override void SetListModel()
        {
            this.ListModel = DAL.MVC_Game2Context.enemieModels;
        }

        public override EnemyViewModel GetDataViewModel(Enemy data)
        {
            return new EnemyViewModel()
            {
                EnemyId = data.EnemyId,
                AttackMax = data.AttackMax,
                AttackMin = data.AttackMin,
                CharacterTypeId = data.CharacterTypeId,
                InitialX = data.InitialX,
                InitialY = data.InitialY,
                LastMoviment = data.LastMoviment,
                CurrentLife = data.CurrentLife,
                MaxLife = data.MaxLife,
                SpeedRun = data.SpeedRun,
                SpeedWalk = data.SpeedWalk
            };
        }

        public override List<EnemyViewModel> GetDataViewModel(List<Enemy> data)
        {
            return (from y in data
                    select new EnemyViewModel()
                    {
                        EnemyId = y.EnemyId,
                        AttackMax = y.AttackMax,
                        AttackMin = y.AttackMin,
                        CharacterTypeId = y.CharacterTypeId,
                        InitialX = y.InitialX,
                        InitialY = y.InitialY,
                        LastMoviment = y.LastMoviment,
                        CurrentLife = y.CurrentLife,
                        MaxLife = y.MaxLife,
                        SpeedRun = y.SpeedRun,
                        SpeedWalk = y.SpeedWalk
                    }).ToList();
        }

        public override Enemy GetDataByViewModel(EnemyViewModel model)
        {
            return new Enemy()
            {
                EnemyId = model.EnemyId,
                AttackMax = model.AttackMax.Value,
                AttackMin = model.AttackMin.Value,
                CharacterTypeId = model.CharacterTypeId.Value,
                InitialX = model.InitialX.Value,
                InitialY = model.InitialY.Value,
                LastMoviment = model.LastMoviment.Value,
                CurrentLife = model.CurrentLife.Value,
                MaxLife = model.MaxLife.Value,
                SpeedRun = model.SpeedRun.Value,
                SpeedWalk = model.SpeedWalk.Value,
            };
        }

        public override List<Enemy> GetDataByViewModel(List<EnemyViewModel> model)
        {
            return (from y in model
                    select new Enemy()
                    {
                        EnemyId = y.EnemyId,
                        AttackMax = y.AttackMax.Value,
                        AttackMin = y.AttackMin.Value,
                        CharacterTypeId = y.CharacterTypeId.Value,
                        InitialX = y.InitialX.Value,
                        InitialY = y.InitialY.Value,
                        LastMoviment = y.LastMoviment.Value,
                        CurrentLife = y.CurrentLife.Value,
                        MaxLife = y.MaxLife.Value,
                        SpeedRun = y.SpeedRun.Value,
                        SpeedWalk = y.SpeedWalk.Value,
                    }).ToList();
        }

        public override void SetListContext()
        {
            this.ListContext = MVC_Game2Context.enemies;
        }
    }
}

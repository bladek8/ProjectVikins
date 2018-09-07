using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.DAL;
using Assets.Script.Helpers;
using Assets.Script.Models;
using UnityEngine;

namespace Assets.Script.BLL
{
    public class EnemyFunctions : BLL.Shared.BLLFunctions<DAL.Enemy,Models.EnemyViewModel>
    {
        public EnemyFunctions()
            : base("EnemyId")
        {
            //SetListContext();
            //SetListModel();
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
                SpeedWalk = model.SpeedWalk.Value,
                IsTank = model.IsTank
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
            ProjectVikingsContext.UpdateAliveLists();
            return model.EnemyId;
        }
        
        public override void SetListModel()
        {
            this.ListModel = DAL.ProjectVikingsContext.enemieModels.Entity;
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
                LastMoviment = (PossibleMoviment)data.LastMoviment,
                CurrentLife = data.CurrentLife,
                MaxLife = data.MaxLife,
                SpeedRun = data.SpeedRun,
                SpeedWalk = data.SpeedWalk,
                IsTank = data.IsTank
            };
        }

        public override List<EnemyViewModel> GetDataViewModel(IEnumerable<Enemy> data)
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
                        LastMoviment = (PossibleMoviment)y.LastMoviment,
                        CurrentLife = y.CurrentLife,
                        MaxLife = y.MaxLife,
                        SpeedRun = y.SpeedRun,
                        SpeedWalk = y.SpeedWalk,
                        IsTank = y.IsTank
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
                LastMoviment = (int)model.LastMoviment.Value,
                CurrentLife = model.CurrentLife.Value,
                MaxLife = model.MaxLife.Value,
                SpeedRun = model.SpeedRun.Value,
                SpeedWalk = model.SpeedWalk.Value,
                IsTank = model.IsTank
            };
        }

        public override List<Enemy> GetDataByViewModel(IEnumerable<EnemyViewModel> model)
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
                        LastMoviment = (int)y.LastMoviment.Value,
                        CurrentLife = y.CurrentLife.Value,
                        MaxLife = y.MaxLife.Value,
                        SpeedRun = y.SpeedRun.Value,
                        SpeedWalk = y.SpeedWalk.Value,
                        IsTank = y.IsTank
                    }).ToList();
        }

        public override void SetListContext()
        {
            this.ListContext = ProjectVikingsContext.Enemy.Data;
        }
    }
}

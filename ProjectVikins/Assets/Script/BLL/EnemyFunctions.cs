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
        }

        public override int Create(Models.EnemyViewModel model)
        {
            var enemy= new DAL.Enemy()
            {
                AttackMax = model.AttackMax.Value,
                CharacterTypeId = model.CharacterTypeId.Value,
                AttackMin = model.AttackMin.Value,
                EnemyId = model.EnemyId,
                Life = model.Life.Value,
                SpeedRun = model.SpeedRun.Value,
                SpeedWalk = model.SpeedWalk.Value
            };
            ListContext.Add(enemy);
            return enemy.EnemyId;
        }

        public override void Decrease(EnemyViewModel model)
        {
            throw new NotImplementedException();
        }

        public override void Increase(EnemyViewModel model)
        {
            throw new NotImplementedException();
        }

        public override void SetListContext()
        {
            this.ListContext = context.GetEnemies();
        }

        public override void UpdateStats(EnemyViewModel model)
        {
            var enemy = this.GetDataById(model.EnemyId);

            if (model.LastMoviment.HasValue) enemy.LastMoviment = model.LastMoviment.Value;
            if (model.Life.HasValue) enemy.Life = model.Life.Value;
            if (model.SpeedRun.HasValue) enemy.SpeedRun = model.SpeedRun.Value;
            if (model.SpeedWalk.HasValue) enemy.SpeedWalk = model.SpeedWalk.Value;
            if (model.AttackMin.HasValue) enemy.AttackMin = model.AttackMin.Value;
            if (model.AttackMax.HasValue) enemy.AttackMax = model.AttackMax.Value;
        }
    }
}

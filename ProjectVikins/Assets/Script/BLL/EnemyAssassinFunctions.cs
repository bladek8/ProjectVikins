using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.BLL.Shared;
using Assets.Script.DAL;
using Assets.Script.Models;

namespace Assets.Script.BLL
{
    public class EnemyAssassinFunctions : BLLFunctions<DAL.EnemyAssassin, Models.EnemyAssassinViewModel>
    {
        public EnemyAssassinFunctions()
            : base("EnemyAssassinId")
        {
        }

        public override int Create(EnemyAssassinViewModel model)
        {
            var enemyAssassin = new DAL.EnemyAssassin()
            {
                AttackMax = model.AttackMax.Value,
                CharacterTypeId = model.CharacterTypeId.Value,
                AttackMin = model.AttackMin.Value,
                EnemyAssassinId = model.EnemyAssassinId.Value,
                Life = model.Life.Value,
                SpeedRun = model.SpeedRun.Value,
                SpeedWalk = model.SpeedWalk.Value
            };
            ListContext.Add(enemyAssassin);
            return enemyAssassin.EnemyAssassinId;
        }

        public override void Decrease(EnemyAssassinViewModel model)
        {
            var enemyAssassin = GetDataById(model.EnemyAssassinId);
            if (model.SpeedWalk.HasValue) enemyAssassin.SpeedWalk = enemyAssassin.SpeedWalk - model.SpeedWalk.Value;
            if (model.AttackMin.HasValue) enemyAssassin.AttackMin = enemyAssassin.AttackMin - model.AttackMin.Value;
            if (model.AttackMax.HasValue) enemyAssassin.AttackMax = enemyAssassin.AttackMax - model.AttackMax.Value;
            if (model.Life.HasValue) enemyAssassin.Life = enemyAssassin.Life - model.Life.Value;
            if (model.SpeedRun.HasValue) enemyAssassin.SpeedRun = enemyAssassin.SpeedRun - model.SpeedRun.Value;
        }

        public override void Increase(EnemyAssassinViewModel model)
        {
            var enemyAssassin = GetDataById(model.EnemyAssassinId);
            if (model.SpeedWalk.HasValue) enemyAssassin.SpeedWalk = enemyAssassin.SpeedWalk + model.SpeedWalk.Value;
            if (model.AttackMin.HasValue) enemyAssassin.AttackMin = enemyAssassin.AttackMin + model.AttackMin.Value;
            if (model.AttackMax.HasValue) enemyAssassin.AttackMax = enemyAssassin.AttackMax + model.AttackMax.Value;
            if (model.Life.HasValue) enemyAssassin.Life = enemyAssassin.Life + model.Life.Value;
            if (model.SpeedRun.HasValue) enemyAssassin.SpeedRun = enemyAssassin.SpeedRun + model.SpeedRun.Value;
        }

        public override void SetListContext()
        {
            this.ListContext = DAL.MVC_Game2Context.enemyAssassins;
        }

        public override void UpdateStats(EnemyAssassinViewModel model)
        {
            var enemyAssassin = GetDataById(model.EnemyAssassinId);
            if (model.SpeedWalk.HasValue) enemyAssassin.SpeedWalk = model.SpeedWalk.Value;
            if (model.AttackMin.HasValue) enemyAssassin.AttackMin = model.AttackMin.Value;
            if (model.AttackMax.HasValue) enemyAssassin.AttackMax = model.AttackMax.Value;
            if (model.Life.HasValue) enemyAssassin.Life = model.Life.Value;
            if (model.SpeedRun.HasValue) enemyAssassin.SpeedRun = model.SpeedRun.Value;
        }
    }
}

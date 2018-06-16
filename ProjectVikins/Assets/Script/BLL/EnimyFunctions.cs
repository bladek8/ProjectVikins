using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.DAL;
using Assets.Script.Models;
using UnityEngine;

namespace Assets.Script.BLL
{
    public class EnimyFunctions : BLL.Shared.BLLFunctions<DAL.Enimy,Models.EnimyViewModel>
    {
        public EnimyFunctions()
            : base("EnimyId")
        {
            SetListContext();
        }

        public override int Create(Models.EnimyViewModel model)
        {
            var enemy= new DAL.Enimy()
            {
                AttackMax = model.AttackMax.Value,
                CharacterTypeId = model.CharacterTypeId.Value,
                AttackMin = model.AttackMin.Value,
                EnimyId = model.EnimyId,
                Life = model.Life.Value,
                SpeedRun = model.SpeedRun.Value,
                SpeedWalk = model.SpeedWalk.Value
            };
            ListContext.Add(enemy);
            return enemy.EnimyId;
        }

        public override void Decrease(EnimyViewModel model)
        {
            throw new NotImplementedException();
        }

        public override void Increase(EnimyViewModel model)
        {
            throw new NotImplementedException();
        }

        public override void SetListContext()
        {
            this.ListContext = context.GetEnimies();
        }

        public override void UpdateStats(EnimyViewModel model)
        {
            var enemy = this.GetDataById(model.EnimyId);

            if (model.LastMoviment.HasValue) enemy.LastMoviment = model.LastMoviment.Value;
            if (model.Life.HasValue) enemy.Life = model.Life.Value;
            if (model.SpeedRun.HasValue) enemy.SpeedRun = model.SpeedRun.Value;
            if (model.SpeedWalk.HasValue) enemy.SpeedWalk = model.SpeedWalk.Value;
            if (model.AttackMin.HasValue) enemy.AttackMin = model.AttackMin.Value;
            if (model.AttackMax.HasValue) enemy.AttackMax = model.AttackMax.Value;
        }
    }
}

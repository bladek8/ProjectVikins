using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.BLL.Shared;
using Assets.Script.DAL;

namespace Assets.Script.BLL
{
    public class EnimyBossFunctions : BLLFunctions<DAL.EnimyBoss>
    {
        public EnimyBossFunctions(int enimyBossId, int characterTypeId, int life, int speedWalk, int speedRun, int attackMin, int attackMax) 
            : base("EnimyBossId")
        {
            SetListContext();
            this.Create(new EnimyBoss
            {
                EnimyBossId = enimyBossId,
                CharacterTypeId = characterTypeId,
                Life = life,
                SpeedWalk = speedWalk,
                SpeedRun = speedRun,
                AttackMin = attackMin,
                AttackMax = attackMax
            });
        }

        public override int Create(EnimyBoss model)
        {
            ListContext.Add(model);
            return model.EnimyBossId;
        }

        public override EnimyBoss GetDataById(int id)
        {
            return ListContext.Where(x => x.EnimyBossId == id).First();
        }

        public override void SetListContext()
        {
            this.ListContext = context.enimyBosses;
        }

        public override void UpdateStats(string stats, object value, int id)
        {
            typeof(DAL.EnimyBoss).GetProperty(stats).SetValue(GetDataById(id), value, null);
        }

        public override void UpdateMultipleStats(Dictionary<string, object> datas, int id)
        {
            foreach(var data in datas)
                typeof(DAL.EnimyBoss).GetProperty(data.Key).SetValue(GetDataById(id), data.Value, null);
        }

        public override void DecreaseStats(string stats, object value, int id)
        {
            int currentStats, _value;
            try
            {
                int.TryParse(typeof(DAL.Enimy).GetProperty(stats).GetValue(GetDataById(id), null).ToString(), out currentStats);
                int.TryParse(value.ToString(), out _value);
            }
            catch
            {
                return;
            }

            typeof(DAL.Enimy).GetProperty(stats).SetValue(GetDataById(id), (currentStats - _value), null);
        }

        public override void IncreaseStats(string stats, object value, int id)
        {
            int currentStats, _value;
            try
            {
                int.TryParse(typeof(DAL.Enimy).GetProperty(stats).GetValue(GetDataById(id), null).ToString(), out currentStats);
                int.TryParse(value.ToString(), out _value);
            }
            catch
            {
                return;
            }

            typeof(DAL.Enimy).GetProperty(stats).SetValue(GetDataById(id), (currentStats + _value), null);
        }
    }
}

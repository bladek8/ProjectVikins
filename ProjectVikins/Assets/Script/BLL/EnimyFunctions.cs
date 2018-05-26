using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.DAL;
using UnityEngine;

namespace Assets.Script.BLL
{
    public class EnimyFunctions : BLL.Shared.BLLFunctions<DAL.Enimy>
    {
        public EnimyFunctions(DAL.Enimy model)
            : base("EnimyId")
        {
            SetListContext();
            this.Create(model);
        }

        public override int Create(Enimy model)
        {
            ListContext.Add(model);
            return model.EnimyId;
        }

        public override Enimy GetDataById(int id)
        {
            return ListContext.Where(x => x.EnimyId == id).FirstOrDefault();
        }
        
        public override void SetListContext()
        {
            this.ListContext = context.enimies;
        }

        public override void UpdateMultipleStats(Dictionary<string, object> datas, int id)
        {
            foreach(var data in datas)
                typeof(DAL.Enimy).GetProperty(data.Key).SetValue(GetDataById(id), data.Value, null);
        }

        public override void UpdateStats(string stats, object value, int id)
        {
            typeof(DAL.Enimy).GetProperty(stats).SetValue(GetDataById(id), value, null);
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


        //public override void Update(Enimy model)
        //{
        //    var enimy = GetDataById(model.EnimyId);

        //    enimy.AttackMax = model.AttackMax;
        //    enimy.AttackMin = model.AttackMin;
        //    enimy.CharacterTypeId = model.CharacterTypeId;
        //    enimy.Life = model.Life;
        //    enimy.SpeedRun = model.SpeedRun;
        //    enimy.SpeedWalk = model.SpeedWalk;
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.DAL;

namespace Assets.Script.BLL
{
    public class PlayerFunctions : Shared.BLLFunctions<DAL.Player>
    {
        public PlayerFunctions(DAL.Player model)
            : base("PlayerId")
        {
            SetListContext();
            Create(model);
        }
        public override int Create(Player model)
        {
            ListContext.Add(model);
            return model.PlayerId;
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

        public override Player GetDataById(int id)
        {
            return ListContext.Where(x => x.PlayerId == id).FirstOrDefault();
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

        public override void SetListContext()
        {
            this.ListContext = context.players;
        }
        
        public override void UpdateMultipleStats(Dictionary<string, object> datas, int id)
        {
            foreach (var data in datas)
                typeof(DAL.Player).GetProperty(data.Key).SetValue(GetDataById(id), data.Value, null);
        }
        
        public override void UpdateStats(string stats, object value, int id)
        {
            typeof(DAL.Player).GetProperty(stats).SetValue(GetDataById(id), value, null);
        }

        //public override void Update(Player model)
        //{
        //    var enimy = GetDataById(model.PlayerId);

        //    enimy.AttackMax = model.AttackMax;
        //    enimy.AttackMin = model.AttackMin;
        //    enimy.CharacterTypeId = model.CharacterTypeId;
        //    enimy.Life = model.Life;
        //    enimy.SpeedRun = model.SpeedRun;
        //    enimy.SpeedWalk = model.SpeedWalk;
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Assets.Script.BLL.Shared
{
    public abstract class BLLFunctions<TEntity> : MonoBehaviour, IBLLFunctions<TEntity>
        where TEntity : class
    {
        protected DAL.MVC_Game2Context context = new DAL.MVC_Game2Context();
        protected List<TEntity> ListContext;
        protected string entityIdPropertyName;

        public BLLFunctions(string entityIdPropertyName)
        {
            this.entityIdPropertyName = entityIdPropertyName;
        }

        public List<TEntity> GetData()
        {
            return ListContext;
        }

        public TEntity GetDataById(int id)
        {
            var idProperty = ListContext[0].GetType().GetProperty(entityIdPropertyName);
            return ListContext.Where(x => Convert.ToInt32(idProperty.GetValue(x, null)) == id).First();

        }
        public void UpdateStats(string stats, object value, int id)
        {
            typeof(TEntity).GetProperty(stats).SetValue(GetDataById(id), value, null);
        }
        public object DecreaseStats(string stats, object value, int id)
        {
            int currentStats, _value, newValue;
            try
            {
                currentStats = Convert.ToInt32(typeof(TEntity).GetProperty(stats).GetValue(GetDataById(id), null));
                _value = Convert.ToInt32(value);
                newValue = (currentStats - _value);
            }
            catch
            {
                return null;
            }

            typeof(DAL.Enimy).GetProperty(stats).SetValue(GetDataById(id), newValue, null);
            return newValue;
        }
        public object IncreaseStats(string stats, object value, int id)
        {
            int currentStats, _value, newValue;
            try
            {
                currentStats = Convert.ToInt32(typeof(TEntity).GetProperty(stats).GetValue(GetDataById(id), null));
                _value = Convert.ToInt32(value);
                newValue = (currentStats + _value);
            }
            catch
            {
                return null;
            }

            typeof(DAL.Enimy).GetProperty(stats).SetValue(GetDataById(id), newValue, null);
            return newValue;
        }
        public void UpdateMultipleStats(Dictionary<string, object> datas, int id)
        {
            foreach (var data in datas)
                typeof(TEntity).GetProperty(data.Key).SetValue(GetDataById(id), data.Value, null);
        }

        public abstract int Create(TEntity model);
        public abstract void SetListContext();
    }
}

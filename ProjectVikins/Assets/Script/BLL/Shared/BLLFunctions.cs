using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Assets.Script.BLL.Shared
{
    public abstract class BLLFunctions<TEntity, TViewModel> : MonoBehaviour, IBLLFunctions<TEntity, TViewModel>
        where TEntity : class
        where TViewModel : class
    {
        //protected DAL.MVC_Game2Context context;
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

        public TEntity GetDataById(object id)
        {
            var idProperty = ListContext[0].GetType().GetProperty(entityIdPropertyName);
            return ListContext.Where(x => int.Parse(idProperty.GetValue(x, null).ToString()) == (int)id).First();

        }
        public TEntity GetDataByInitialPosition(Vector3 initialPosition)
        {
            Vector2 vector2 = initialPosition;
            var initialX = ListContext[0].GetType().GetProperty("InitialX");
            var initialY = ListContext[0].GetType().GetProperty("InitialY");
            foreach(var a in ListContext)
            {
                string b = initialX.GetValue(a, null).ToString();
                string c = initialY.GetValue(a, null).ToString();
                var d = new Vector2(Convert.ToInt32(b), Convert.ToInt32(c));
                if (d == vector2)
                    return a;
            }
            return null;
            
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

            typeof(DAL.Enemy).GetProperty(stats).SetValue(GetDataById(id), newValue, null);
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

            typeof(DAL.Enemy).GetProperty(stats).SetValue(GetDataById(id), newValue, null);
            return newValue;
        }
        public void UpdateMultipleStats(Dictionary<string, object> datas, int id)
        {
            foreach (var data in datas)
                typeof(TEntity).GetProperty(data.Key).SetValue(GetDataById(id), data.Value, null);
        }
        public Dictionary<string, object> DecreaseMultipleStats(Dictionary<string, object> datas, int id)
        {
            int currentStats, _value, newValue;
            var newDatas = new Dictionary<string, object>();
            var model = GetDataById(id);

            foreach (KeyValuePair<string, object> data in datas)
            {
                try
                {
                    currentStats = Convert.ToInt32(typeof(TEntity).GetProperty(data.Key).GetValue(model, null));
                    _value = Convert.ToInt32(data.Value);
                    newValue = (currentStats - _value);
                }
                catch
                {
                    return null;
                }

                typeof(DAL.Enemy).GetProperty(data.Key).SetValue(GetDataById(id), newValue, null);
                newDatas.Add(data.Key, newValue);
            }

            return newDatas;
        }
        public Dictionary<string, object> IncreaseMultipleStats(Dictionary<string, object> datas, int id)
        {
            int currentStats, _value, newValue;
            var newDatas = new Dictionary<string, object>();

            foreach (var data in datas)
            {
                try
                {
                    currentStats = Convert.ToInt32(typeof(TEntity).GetProperty(data.Key).GetValue(GetDataById(id), null));
                    _value = Convert.ToInt32(data.Value);
                    newValue = (currentStats + _value);
                }
                catch
                {
                    return null;
                }

                typeof(DAL.Enemy).GetProperty(data.Key).SetValue(GetDataById(id), newValue, null);
                newDatas.Add(data.Key, newValue);
            }

            return newDatas;
        }

        public abstract int Create(TViewModel model);
        public abstract void SetListContext();
        public abstract void UpdateStats(TViewModel model);
        public abstract void Decrease(TViewModel model);
        public abstract void Increase(TViewModel model);
        //public abstract TViewModel GetDataViewModel(TEntity data);
    }
}

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

        public abstract TEntity GetDataById(int id);
        public abstract int Create(TEntity model);
        public abstract void SetListContext();
        public abstract void UpdateStats(string stats, object value, int id);
        public abstract void DecreaseStats(string stats, object value, int id);
        public abstract void IncreaseStats(string stats, object value, int id);
        public abstract void UpdateMultipleStats(Dictionary<string, object> datas, int id);


        //public abstract void UpdateStats(IEnumerable<string> stats, object value, int id)
    }
}

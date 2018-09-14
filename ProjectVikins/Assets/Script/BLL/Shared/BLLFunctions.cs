using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Assets.Script.BLL.Shared
{
    public abstract class BLLFunctions<TEntity, TViewModel> : IBLLFunctions<TEntity, TViewModel>
        where TEntity : class
        where TViewModel : class
    {
        protected List<TEntity> ListContext;
        protected List<TViewModel> ListModel;
        protected string entityIdPropertyName;

        public BLLFunctions(string entityIdPropertyName)
        {
            SetListContext();
            SetListModel();
            this.entityIdPropertyName = entityIdPropertyName;
        }

        public List<TEntity> GetData()
        {
            return ListContext;
        }

        public TEntity GetDataById(object id)
        {
            var idProperty = typeof(TEntity).GetProperties().SingleOrDefault(x => x.Name == entityIdPropertyName);
            return ListContext.SingleOrDefault(x => int.Parse(idProperty.GetValue(x, null).ToString()) == (int)id);
        }

        public List<TViewModel> GetModels()
        {
            return ListModel;
        }

        public TViewModel GetModelById(object id)
        {
            var idProperty = (typeof(TViewModel)).GetProperties().SingleOrDefault(x => x.Name == "Internal" + entityIdPropertyName);
            return ListModel.SingleOrDefault(x => int.Parse(idProperty.GetValue(x, null).ToString()) == (int)id);
        }

        public abstract int Create(TViewModel model);
        public abstract void SetListContext();
        public abstract void SetListModel();
        public abstract int SetModel(TViewModel model);
        public abstract TViewModel GetDataViewModel(TEntity data);
        public abstract List<TViewModel> GetDataViewModel(IEnumerable<TEntity> data);
        public abstract TEntity GetDataByViewModel(TViewModel model);
        public abstract List<TEntity> GetDataByViewModel(IEnumerable<TViewModel> model);

    }
}

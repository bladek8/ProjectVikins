﻿using System;
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

        public List<TViewModel> GetModels()
        {
            return ListModel;
        }

        public TViewModel GetModelById(object id)
        {
            var idProperty = ListModel[0].GetType().GetProperty(entityIdPropertyName);
            return ListModel.Where(x => int.Parse(idProperty.GetValue(x, null).ToString()) == (int)id).First();
        }

        public TEntity GetDataByInitialPosition(Vector3 initialPosition)
        {
            Vector2 vector2 = initialPosition;
            var initialX = ListContext[0].GetType().GetProperty("InitialX");
            var initialY = ListContext[0].GetType().GetProperty("InitialY");
            foreach(var a in ListContext)
            {
                var b = initialX.GetValue(a, null);
                var c = initialY.GetValue(a, null);
                var d = new Vector2((float)b, (float)c);
                if (d == vector2)
                    return a;
            }
            return null;
            
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

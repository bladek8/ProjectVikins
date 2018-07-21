using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.BLL.Shared
{
    public abstract class BLLListFunctions<TEntity, TViewModel>
        where TEntity : class
        where TViewModel : class
    {
        protected List<TEntity> ListContext;
        protected string entityIdPropertyName;

        public BLLListFunctions(string entityIdPropertyName)
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
            foreach (var a in ListContext)
            {
                var b = initialX.GetValue(a, null);
                var c = initialY.GetValue(a, null);
                var d = new Vector2((float)b, (float)c);
                if (d == vector2)
                    return a;
            }
            return null;

        }
        public abstract void SetListContext();
        public abstract TViewModel GetDataViewModel(TEntity data);
        public abstract List<TViewModel> GetDataViewModel(IEnumerable<TEntity> data);
    }
}

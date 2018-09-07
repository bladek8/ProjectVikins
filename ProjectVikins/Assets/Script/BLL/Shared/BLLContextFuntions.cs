using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.BLL.Shared
{
    public abstract class BLLContextFuntions<TEntity>
        where TEntity : class
    {
        protected List<TEntity> ListContext;
        protected string entityIdPropertyName;

        public BLLContextFuntions(string entityIdPropertyName)
        {
            SetListContext();
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
        public abstract int Create(TEntity data);
        public abstract void SetListContext();

    }
}

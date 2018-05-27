using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Script.BLL.Shared
{
    public interface IBLLFunctions<TEntity>
        where TEntity : class
    {
        List<TEntity> GetData();
        TEntity GetDataById(int id);
        int Create(TEntity model);
        void UpdateStats(string stats, object value, int id);
        object DecreaseStats(string stats, object value, int id);
        object IncreaseStats(string stats, object value, int id);
        void UpdateMultipleStats(Dictionary<string, object> datas, int id);
        void SetListContext();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Script.BLL.Shared
{
    public interface IBLLFunctions<TEntity, TViewModel>
        where TEntity : class
        where TViewModel : class
    {
        List<TEntity> GetData();
        List<TViewModel> GetModels();
        TEntity GetDataById(object id);
        int SetModel(TViewModel model);
        TViewModel GetModelById(object id);
        TViewModel GetDataViewModel(TEntity data);
        List<TViewModel> GetDataViewModel(List<TEntity> data);
        TEntity GetDataByViewModel(TViewModel model);
        List<TEntity> GetDataByViewModel(List<TViewModel> model);
        int Create(TViewModel model);
        void UpdateStats(string stats, object value, int id);
        object DecreaseStats(string stats, object value, int id);
        object IncreaseStats(string stats, object value, int id);
        void UpdateMultipleStats(Dictionary<string, object> datas, int id);
        void SetListContext();
        void SetListModel();
    }
}
